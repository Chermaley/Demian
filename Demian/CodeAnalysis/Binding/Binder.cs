using System.Collections.Immutable;
using Demian.CodeAnalysis.Syntax;

namespace Demian.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        private BoundScope _scope;
        public Binder(BoundScope parent)
        {
            _scope = new BoundScope(parent);
        }
        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax)
        {
            var parentScope = CreateParentScopes(previous);
            var binder = new Binder(parentScope);
            var expression = binder.BindStatement(syntax.Statement);
            var variables = binder._scope.GetDeclaredVariables();
            var diagnostics = binder.Diagnostics.ToImmutableArray();

            if (previous != null)
                diagnostics = diagnostics.InsertRange(0, previous.Diagnostics);
            
            return new BoundGlobalScope(previous, diagnostics, variables, expression);
        }
        private static BoundScope CreateParentScopes(BoundGlobalScope previous)
        {
            var stack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                stack.Push(previous);
                previous = previous.Previous;
            }

            BoundScope parent = null;

            while (stack.Count > 0)
            {
                previous = stack.Pop();
                var scope = new BoundScope(parent);
                foreach (var v in previous.Variables)
                    scope.TryDeclare(v);
                parent = scope;
            }

            return parent;
        }
        public DiagnosticBag Diagnostics => _diagnostics;
        private BoundStatement BindStatement(StatementSyntax syntax)
        {
            switch (syntax.Kind) 
            { 
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement((BlockStatementSyntax)syntax);
                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement((ExpressionStatementSyntax)syntax);             
                case SyntaxKind.VariableDeclaration:
                    return BindVariableDeclaration((VariableDeclarationSyntax)syntax);
                case SyntaxKind.IfStatement:
                    return BindIfStatement((IfStatementSyntax)syntax);
                case SyntaxKind.WhileStatement:
                    return BindWhileStatement((WhileStatementSyntax)syntax);
                case SyntaxKind.ForStatement:
                    return BindForStatement((ForStatementSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }
        private BoundStatement BindBlockStatement(BlockStatementSyntax syntax)
        {
            var statements = ImmutableArray.CreateBuilder<BoundStatement>();
            _scope = new BoundScope(_scope);
            
            foreach (var statementSyntax in syntax.Statements)
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }

            _scope = _scope.Parent;
            return new BoundBlockStatement(statements.ToImmutable());
        }
        private BoundStatement BindExpressionStatement(ExpressionStatementSyntax syntax)
        {
            var expression = BindExpression(syntax.Expression);

            return new BoundExpressionStatement(expression);
        }
        private BoundStatement BindVariableDeclaration(VariableDeclarationSyntax syntax)
        {
            var name = syntax.Identifier.Text;
            var expression = BindExpression(syntax.Initializer);
            var isReadOnly = syntax.Keyword.Kind == SyntaxKind.LetKeyword;
            var variable = new VariableSymbol(name, isReadOnly, expression.Type);
            
            if (!_scope.TryDeclare(variable))
                _diagnostics.ReportVariableAlreadyDeclared(syntax.Identifier.Span, name);
            
            return new BoundVariableDeclaration(variable, expression);
        }
        private BoundStatement BindIfStatement(IfStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, typeof(bool));
            var thenStatement = BindStatement(syntax.ThenStatement);
            var elseStatement = syntax.ElseClause == null ? null : BindStatement(syntax.ElseClause.ElseStatement);
            
            return new BoundIfStatement(condition, thenStatement, elseStatement);
        }
        private BoundStatement BindWhileStatement(WhileStatementSyntax syntax)
        {
            var condition = BindExpression(syntax.Condition, typeof(bool));
            var body = BindStatement(syntax.Statement);

            return new BoundWhileStatement(condition, body);
        }
        private BoundStatement BindForStatement(ForStatementSyntax syntax)
        {
            var lowerBound = BindExpression(syntax.LowerBound, typeof(int));
            var upperBound = BindExpression(syntax.UpperBound, typeof(int));
            
            _scope = new BoundScope(_scope);

            var name = syntax.Identifier.Text;
            var variable = new VariableSymbol(name, true, typeof(int));
            if (!_scope.TryDeclare(variable))
                _diagnostics.ReportVariableAlreadyDeclared(syntax.Identifier.Span, name);

            var body = BindStatement(syntax.Body);

            _scope = _scope.Parent;

            return new BoundForStatement(variable, lowerBound, upperBound, body);
        }
        private BoundExpression BindExpression(ExpressionSyntax syntax, Type targetType)
        {
            var result = BindExpression(syntax);
            if (result.Type != targetType) 
                _diagnostics.ReportVariableCannotConvert(syntax.Span, result.Type, targetType);
            return result;
        }
        private BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind) 
            { 
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionsSyntax)syntax);
                case SyntaxKind.AssigmentExpression:
                    return BindAssigmentExpression((AssigmentExpressionSyntax)syntax); 
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }
        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }
        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            if (string.IsNullOrEmpty(name))
            {
                return new BoundLiteralExpression(0);
            }
            if (!_scope.TryLookup(name, out var variable))
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new BoundLiteralExpression(0);
            }
            return new BoundVariableExpression(variable);
        }
        private BoundExpression BindAssigmentExpression(AssigmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);

            if (!_scope.TryLookup(name, out var variable))
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return boundExpression;
            }

            if (variable.IsReadOnly)
            {
                _diagnostics.ReportCannotAssign(syntax.EqualsToken.Span, name);
                return boundExpression;
            }

            if (boundExpression.Type != variable.Type)
            {
                _diagnostics.ReportVariableCannotConvert(syntax.Expression.Span, boundExpression.Type, variable.Type);
                return boundExpression;
            }
            
            return new BoundAssigmentExpression(variable, boundExpression);
        }
        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);
            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
                return boundOperand;
            }           
            return new BoundUnaryExpression(boundOperator, boundOperand);
        }
        private BoundExpression BindBinaryExpression(BinaryExpressionsSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
                return boundLeft;
            }       
            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
    
}