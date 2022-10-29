namespace Demian.CodeAnalysis.Syntax;

public sealed class CompilationUnitSyntax : SyntaxNode
{
    public CompilationUnitSyntax(ExpressionSyntax expression, SyntaxNode endOfFileToken)
    {
        Expression = expression;
        EndOfFileToken = endOfFileToken;
    }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode EndOfFileToken { get; }
    public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
}