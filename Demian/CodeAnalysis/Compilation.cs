using System.Collections.Immutable;
using Demian.CodeAnalysis.Binding;
using Demian.CodeAnalysis.Syntax;

namespace Demian.CodeAnalysis;

public sealed class Compilation
{
    private BoundGlobalScope _globalScope;
    public Compilation Previous { get; }
    public SyntaxTree Syntax { get; }
    public Compilation(SyntaxTree syntaxTree)
        :this(null, syntaxTree)
    {
        Syntax = syntaxTree;
    }
    private Compilation(Compilation previous, SyntaxTree syntax)
    {
        Previous = previous;
        Syntax = syntax;
    }
    internal BoundGlobalScope GlobalScope
    {
        get
        {
            if (_globalScope == null)
            {
                var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, Syntax.Root);
                Interlocked.CompareExchange(ref _globalScope, globalScope, null); 
            }

            return _globalScope;
        }
    }
    public Compilation ContinueWith(SyntaxTree syntaxTree)
    {
        return new Compilation(this, syntaxTree);
    }
    public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
    {
        var diagnostics = Syntax.Diagnostics.Concat(GlobalScope.Diagnostics).ToArray();
        if (diagnostics.Any())
        {
            return new EvaluationResult(diagnostics.ToImmutableArray(), null);
        }
        var evaluator = new Evaluator(GlobalScope.Statement, variables);
        var value = evaluator.Evaluate();
        return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
    }
}