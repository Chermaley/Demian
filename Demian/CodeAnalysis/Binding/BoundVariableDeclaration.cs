namespace Demian.CodeAnalysis.Binding;

internal sealed class BoundVariableDeclaration : BoundStatement
{
    public BoundVariableDeclaration(VariableSymbol variable, BoundExpression initializer)
    {
        Variable = variable;
        Initializer = initializer;
    }
    public VariableSymbol Variable { get; }
    public bool IsReadOnly { get; }
    public BoundExpression Initializer { get; }
    public override BoundNodeKind Kind => BoundNodeKind.VariableDeclaration;
}