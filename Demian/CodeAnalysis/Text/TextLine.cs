namespace Demian.CodeAnalysis.Text;

public sealed class TextLine
{
    public TextLine(SourceText text, int start, int length, int lenghtIncludingLineBreak)
    {
        Text = text;
        Start = start;
        Length = length;
        LenghtIncludingLineBreak = lenghtIncludingLineBreak;
    }
    public SourceText Text { get; }
    public int Start { get; }
    public int Length { get; }
    public int End => Start + Length;
    public int LenghtIncludingLineBreak { get; }
    public TextSpan Span => new (Start, Length);
    public TextSpan SpanIncludingLineBreak => new (Start, LenghtIncludingLineBreak);
    public override string ToString() => Text.ToString(Span);
}