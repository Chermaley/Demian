namespace Demian.CodeAnalysis.Syntax;

public sealed class ElseClauseStatement : SyntaxNode
{
    public ElseClauseStatement(SyntaxToken elseKeyword, StatementSyntax elseStatement)
    {
        ElseKeyword = elseKeyword;
        ElseStatement = elseStatement;
    }
    public SyntaxToken ElseKeyword { get; }
    public StatementSyntax ElseStatement { get; }
    public override SyntaxKind Kind { get; }
}