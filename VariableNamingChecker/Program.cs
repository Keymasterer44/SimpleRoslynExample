using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Text;

namespace VariableNamingChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a path to a C# file as a command argument!");
            }
            string targetFilePath = args[0];
            string targetFileContents = File.ReadAllText(targetFilePath);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(targetFileContents);
            CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();
            List<VariableDeclaratorSyntax> declarations = GetVariableDeclarations(root);
            List<VariableDeclaratorSyntax> badDeclarations = FindPoorlyNamedDeclarations(declarations);
            foreach (VariableDeclaratorSyntax declaration in badDeclarations)
            {
                var linePos = declaration.GetLocation().GetLineSpan().StartLinePosition;
                Console.WriteLine($"Variable at line {linePos.Line+1} col {linePos.Character+1} is called \"{declaration.Identifier.ValueText}\", recommended name is \"{ToCamelCase(declaration.Identifier.ValueText)}\"!");
            }
        }

        private static List<VariableDeclaratorSyntax> GetVariableDeclarations(SyntaxNode root)
        {
            List<VariableDeclaratorSyntax> declarations = new List<VariableDeclaratorSyntax>();
            foreach (var node in root.DescendantNodes())
            {
                if (node is VariableDeclaratorSyntax declaration)
                {
                    declarations.Add(declaration);
                }
            }
            return declarations;
        }

        private static List<VariableDeclaratorSyntax> FindPoorlyNamedDeclarations(List<VariableDeclaratorSyntax> declarations)
        {
            return declarations.Where(d => d.Identifier.ValueText.Any((c) => !char.IsLetter(c)) || char.IsUpper(d.Identifier.ValueText.FirstOrDefault())).ToList();
        }

        private static string ToCamelCase(string value)
        {
            StringBuilder res = new StringBuilder();
            bool nextCharShouldBeUpperCase = false;
            foreach (char c in value)
            {
                if (!nextCharShouldBeUpperCase)
                {
                    if (char.IsLetter(c))
                    {
                        res.Append(char.ToLower(c));
                    }
                    else
                    {
                        nextCharShouldBeUpperCase = true;
                    }
                }
                else
                {
                    if (char.IsLetter(c))
                    {
                        res.Append(char.ToUpper(c));
                        nextCharShouldBeUpperCase = false;
                    }
                }
            }
            return res.ToString();
        }
    }
}
