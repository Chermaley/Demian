namespace Demian.CodeAnalysis.Binding;

public enum BoundNodeKind
{
    //Statements
    BlockStatement,
    ExpressionStatement,
    VariableDeclaration,
    IfStatement,
    
    //Expressions
    UnaryExpression,
    LiteralExpression,
    BinaryExpression,
    VariableExpression,
    AssigmentExpression,
}