using Demian.CodeAnalysis.Syntax;

internal class WhileStatement : StatementSyntax
{ 
    public WhileStatement(SyntaxToken whileKeyword, ExpressionSyntax condition, StatementSyntax statement)
    {
        WhileKeyword = whileKeyword;
        Condition = condition;
        Statement = statement;
    }
    public override SyntaxKind Kind => SyntaxKind.WhileStatement;
    public SyntaxToken WhileKeyword { get; }
    public ExpressionSyntax Condition { get; }
    public StatementSyntax Statement { get; }
}