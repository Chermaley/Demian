using Demian.CodeAnalysis.Syntax;
namespace Demian.CodeAnalysis.Binding;

internal sealed class BoundUnaryOperator
{
    private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType)
        : this(syntaxKind, kind, operandType, operandType)
    {
    }
    
    public BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type type)
    {
        SyntaxKind = syntaxKind;
        Kind = kind;
        OperandType = operandType;
        Type = type;
    }
    public SyntaxKind SyntaxKind { get; }
    public BoundUnaryOperatorKind Kind { get; }
    public Type Type { get; }
    public Type OperandType { get; }

    private static BoundUnaryOperator[] _operators =
    {
        new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
        new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
        new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
        new BoundUnaryOperator(SyntaxKind.TildeToken, BoundUnaryOperatorKind.OnesComplement, typeof(int)),
    };

    public static BoundUnaryOperator? Bind(SyntaxKind syntaxKind, Type operatorType)
    {
        foreach (var op in _operators)
        {
            if (op.SyntaxKind == syntaxKind && op.OperandType == operatorType)
                return op;
        }
        return null;
    }
}