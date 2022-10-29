namespace Demian.CodeAnalysis.Syntax;
public enum SyntaxKind
{
    //Tokens
    NumberToken,
    WhiteSpaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    BadToken,
    EndOfFileToken,
    IdentifierToken,
    BangToken,
    AmpersandAmpersandToken,
    PipePipeToken,
    EqualsEqualsToken,
    BangEqualsToken,
    EqualsToken,

    //Expressions 
    UnaryExpression,
    LiteralExpression,
    ParenthesizedExpression,
    BinaryExpression,
    NameExpression,
    AssigmentExpression,
    
    // Keywords
    TrueKeyword,
    FalseKeyword,
    
    //Nodes 
    CompilationUnit
} 