using LetterPress.Deboss;
using LetterPress.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LetterPress {
  public class Press {

    public Dictionary<string, ClassDeclarationSyntax> LoadClassesFromDirectory(string directoryPath) {
      var classDeclarations = new Dictionary<string, ClassDeclarationSyntax>();

      // Enumerate all C# files in the directory
      var filePaths = Directory.EnumerateFiles(directoryPath, "*.cs", SearchOption.TopDirectoryOnly);

      foreach (var filePath in filePaths) {
        // Parse the syntax tree for each document
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
        var root = syntaxTree.GetRoot();

        // Extract all class declarations in the current document
        var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

        foreach (var classDecl in classes) {
          // Use the class name as the key and the class declaration as the value
          classDeclarations[classDecl.Identifier.Text] = classDecl;
        }
      }

      return Classes = classDeclarations;
    }
    public Dictionary<string, ClassDeclarationSyntax> Classes { get; set; }
    public Press() {
      Classes = new Dictionary<string, ClassDeclarationSyntax>();
    }
    public Press(Dictionary<string, ClassDeclarationSyntax> classes) {
      Classes = classes;
    }
  }
}
