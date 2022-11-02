namespace Demian.CodeAnalysis.Binding;

internal class BoundWhileStatement : BoundStatement
{
    public BoundWhileStatement(BoundExpression condition, BoundStatement statement)
    {
        Condition = condition;
        Statement = statement;
    }

    public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
    public BoundExpression Condition { get; }
    public BoundStatement Statement { get; }
}