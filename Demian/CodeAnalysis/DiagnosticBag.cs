using System.Collections;
using Demian.CodeAnalysis.Syntax;
using Demian.CodeAnalysis.Text;

namespace Demian.CodeAnalysis;

public sealed class DiagnosticBag: IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
    public void AddRange(DiagnosticBag diagnostics)
    {
        _diagnostics.AddRange(diagnostics._diagnostics);
    }
    private void Report(TextSpan span, string message)
    {
        var diagnostic = new Diagnostic(span, message);
        _diagnostics.Add(diagnostic);
    }

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void ReportInvalidNumber(TextSpan span, string text, Type type)
    {
        var message = $"The number {text} isn`t valid {type}.";
        Report(span, message);
    }

    public void ReportBadCharacter(int position, char character)
    {
        var span = new TextSpan(position, 1);
        var message = $"Bad character in input: '{character}'.";
        Report(span, message);
    }

    public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind)
    {
        var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>.";
        Report(span, message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type operatorType)
    {
        var message = $"Unary operator '{operatorText}' is not defined for type '{operatorType}'.";
        Report(span, message);
    }

    public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType)
    {
        var message = $"Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'.";
        Report(span, message);
    }

    public void ReportUndefinedName(TextSpan span, string name)
    {
        var message = $"Variable '{name}' doesn't exist.";
        Report(span, message);    
    }
    
    public void ReportVariableCannotConvert(TextSpan span, Type actualType, Type expectedType)
    {
        var message = $"Variable type '{actualType}' is not assignable to '{expectedType}'.";
        Report(span, message); 
    }

    public void ReportVariableAlreadyDeclared(TextSpan span, string name)
    {
        var message = $"Variable '{name}' is already declared.";
        Report(span, message);
    }

    public void ReportCannotAssign(TextSpan span, string name)
    {
        var message = $"Variable '{name}' is read-only and cannot be assigned to.";
        Report(span, message);
    }
}