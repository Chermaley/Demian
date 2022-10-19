using Demian.CodeAnalysis.Syntax;

namespace Demian.CodeAnalysis.Binding;

internal sealed class BoundAssigmentExpression : BoundExpression
{
    public BoundAssigmentExpression(VariableSymbol variable, BoundExpression expression)
    {
        Variable = variable;
        Expression = expression;
    }
    public override BoundNodeKind Kind => BoundNodeKind.AssigmentExpression;
    public VariableSymbol Variable { get; }
    public BoundExpression Expression { get; }
    public override Type Type => Expression.Type;
}