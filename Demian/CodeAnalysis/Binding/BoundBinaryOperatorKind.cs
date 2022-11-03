namespace Demian.CodeAnalysis.Binding;

internal enum BoundBinaryOperatorKind
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    LogicalAnd,
    BitwiseAnd,
    LogicalOr,
    BitwiseOr,
    BitwiseXor,
    Equals,
    NotEquals,
    LogicalLess,
    LogicalLessOrEquals,
    LogicalGreat,
    LogicalGreatOrEquals,
}