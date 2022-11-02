namespace Demian.CodeAnalysis.Binding;

public enum BoundNodeKind
{
    //Statements
    BlockStatement,
    ExpressionStatement,
    VariableDeclaration,
    IfStatement,
    WhileStatement,

    //Expressions
    UnaryExpression,
    LiteralExpression,
    BinaryExpression,
    VariableExpression,
    AssigmentExpression,
}