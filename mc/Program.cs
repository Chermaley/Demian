using System;
using System.Text;
using Demian.CodeAnalysis;
using Demian.CodeAnalysis.Syntax;
using Demian.CodeAnalysis.Text;

namespace Compiller
{
    internal static class Program
    {
        private static void Main()
        {
            var variables = new Dictionary<VariableSymbol, object>();
            var showTree = false;
            var showProgramm = false;
            var textBuilder = new StringBuilder();
            Compilation previous = null;
            
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                
                if (textBuilder.Length == 0)
                    Console.Write("> ");
                else
                    Console.Write("| ");

                Console.ResetColor();

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);
                
                if (textBuilder.Length == 0)
                {
                    if (isBlank) 
                        break;
                    if (input == "#showTree")
                    {
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing parse trees" : "Not showing parse trees");
                        continue;
                    }
                    
                    if (input == "#showProgram")
                    {
                        showProgramm = !showProgramm;
                        Console.WriteLine(showProgramm ? "Showing bound trees" : "Not showing bound trees");
                        continue;
                    }
                
                    if (input == "#cls") 
                    {
                        Console.Clear();
                        continue;
                    }
                    
                    if (input == "#reset")
                    {
                        previous = null;
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();
                
                var syntaxTree = SyntaxTree.Parse(text);
                
                if (!isBlank && syntaxTree.Diagnostics.Any())
                {
                    continue;
                }
                
                var compilation = previous == null ? new Compilation(syntaxTree) : previous.ContinueWith(syntaxTree);
                
                var result = compilation.Evaluate(variables);
                
                if (showTree)
                    syntaxTree.Root.WriteTo(Console.Out);
                
                if (showProgramm)
                    compilation.EmitTree(Console.Out);
                
                if (result.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in result.Diagnostics)
                    {
                        var lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                        var lineNumber = lineIndex + 1;
                        var line = syntaxTree.Text.Lines[lineIndex];
                        var character = diagnostic.Span.Start - line.Start + 1;
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                        var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);
                        
                        var prefix = syntaxTree.Text.ToString(prefixSpan);
                        var error = syntaxTree.Text.ToString(diagnostic.Span);
                        var suffix = syntaxTree.Text.ToString(suffixSpan);

                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);

                        Console.WriteLine();
                    }
                    
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(result.Value);

                    previous = compilation;
                }

                textBuilder.Clear();
            }
        }
    }
}