namespace Demian.CodeAnalysis.Syntax;
public sealed class UnaryExpressionsSyntax : ExpressionSyntax
{
    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Operand { get; }

    public UnaryExpressionsSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
    {
        OperatorToken = operatorToken;
        Operand = operand;
    }
        
    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Operand;
        yield return OperatorToken;
    }
}