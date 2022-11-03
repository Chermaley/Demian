using Demian.CodeAnalysis.Syntax;

namespace Demian.CodeAnalysis.Binding;

internal sealed class BoundBinaryOperator
{
    public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type)
        : this(syntaxKind, kind, type, type, type)
    {
    }
    public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType)
    : this(syntaxKind, kind,operandType, operandType, resultType)
    {
    }
    public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type type)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        LeftType = leftType;
        RightType = rightType;
        Type = type;
    }
    public SyntaxKind SyntaxKind { get; }
    public BoundBinaryOperatorKind Kind { get; }
    public Type LeftType { get; }
    public Type RightType { get; }
    public Type Type { get; }

    private static BoundBinaryOperator[] _operators =
    {
        new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
        new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),    
        new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
        new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),
        
        new BoundBinaryOperator(SyntaxKind.AmpersandToken, BoundBinaryOperatorKind.BitwiseAnd, typeof(int)),
        new BoundBinaryOperator(SyntaxKind.PipeToken, BoundBinaryOperatorKind.BitwiseOr, typeof(int)),
        new BoundBinaryOperator(SyntaxKind.HatToken, BoundBinaryOperatorKind.BitwiseXor, typeof(int)),
        
        new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(int), typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.LessToken, BoundBinaryOperatorKind.LogicalLess, typeof(int), typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.LessOrEqualsToken, BoundBinaryOperatorKind.LogicalLessOrEquals, typeof(int), typeof(bool)), 
        new BoundBinaryOperator(SyntaxKind.GreaterToken, BoundBinaryOperatorKind.LogicalGreat, typeof(int), typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.GreaterOrEqualsToken, BoundBinaryOperatorKind.LogicalGreatOrEquals, typeof(int), typeof(bool)),
        
        new BoundBinaryOperator(SyntaxKind.AmpersandToken, BoundBinaryOperatorKind.BitwiseAnd, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.PipeToken, BoundBinaryOperatorKind.BitwiseOr, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.HatToken, BoundBinaryOperatorKind.BitwiseXor, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(bool)),
        new BoundBinaryOperator(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(bool)),
    };

    public static BoundBinaryOperator? Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.RightType == leftType && op.LeftType == rightType)
                return op;
        }
        return null;
    }
}