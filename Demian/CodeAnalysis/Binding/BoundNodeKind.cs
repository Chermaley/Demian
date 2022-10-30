namespace Demian.CodeAnalysis.Binding;

public enum BoundNodeKind
{
    //Statements
    BlockStatement,
    ExpressionStatement,
    VariableDeclaration,
    //Expressions
    UnaryExpression,
    LiteralExpression,
    BinaryExpression,
    VariableExpression,
    AssigmentExpression,
}