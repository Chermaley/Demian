namespace Demian.CodeAnalysis.Syntax;

public sealed class IfStatementSyntax : StatementSyntax
{
    public IfStatementSyntax(SyntaxToken ifKeyword, ExpressionSyntax condition, StatementSyntax thenStatement, ElseClauseStatement elseClause)
    {
        IfKeyword = ifKeyword;
        Condition = condition;
        ThenStatement = thenStatement;
        ElseClause = elseClause;
    }
    public SyntaxToken IfKeyword { get; }
    public ExpressionSyntax Condition { get; }
    public StatementSyntax ThenStatement { get; }
    public ElseClauseStatement ElseClause { get; }
    public override SyntaxKind Kind => SyntaxKind.IfStatement;
}