namespace Demian.CodeAnalysis.Syntax;

public sealed class CompilationUnitSyntax : SyntaxNode
{
    public CompilationUnitSyntax(StatementSyntax statement, SyntaxNode endOfFileToken)
    {
        Statement = statement;
        EndOfFileToken = endOfFileToken;
    }
    public StatementSyntax Statement { get; }
    public SyntaxNode EndOfFileToken { get; }
    public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
}