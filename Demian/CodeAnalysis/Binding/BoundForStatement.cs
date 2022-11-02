using Demian.CodeAnalysis;
using Demian.CodeAnalysis.Binding;
using Demian.CodeAnalysis.Syntax;

internal class BoundForStatement : BoundStatement
{
    public BoundForStatement(VariableSymbol variable, BoundExpression lowerBound, BoundExpression upperBound, BoundStatement body)
    {
        Variable = variable;
        LowerBound = lowerBound;
        UpperBound = upperBound;
        Body = body;
    }
    public override BoundNodeKind Kind => BoundNodeKind.ForStatement;
    public VariableSymbol Variable { get; }
    public BoundExpression LowerBound { get; }
    public BoundExpression UpperBound { get; }
    public BoundStatement Body { get; }

}