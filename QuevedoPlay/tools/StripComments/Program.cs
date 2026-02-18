using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace StripCommentsTool
{
    class CommentRemover : CSharpSyntaxRewriter
    {
        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
                trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia) ||
                trivia.IsKind(SyntaxKind.DocumentationCommentExteriorTrivia))
            {
                return default(SyntaxTrivia);
            }

            return trivia;
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: StripComments <path>");
                return 1;
            }

            string root = args[0];
            var csFiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);

            var remover = new CommentRemover();

            foreach (var file in csFiles)
            {
                try
                {
                    var text = File.ReadAllText(file);
                    var tree = CSharpSyntaxTree.ParseText(text);
                    var rootNode = tree.GetRoot();

                    var newRoot = remover.Visit(rootNode);

                    File.WriteAllText(file, newRoot.ToFullString());
                    Console.WriteLine("Stripped comments from: " + file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process {file}: {ex.Message}");
                }
            }

            return 0;
        }
    }
}
