namespace Demian.CodeAnalysis.Binding;

public enum BoundNodeKind
{
    //Statements
    BlockStatement,
    ExpressionStatement,
    //Expressions
    UnaryExpression,
    LiteralExpression,
    BinaryExpression,
    VariableExpression,
    AssigmentExpression,
}