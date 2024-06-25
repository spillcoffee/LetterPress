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
using LetterPress.Forme;

namespace LetterPress {
  public class Press {
    public Press(Dictionary<string, ClassDeclarationSyntax> classes) {
      Classes = classes;
      Init();
    }
    public Press() {
      Classes = new Dictionary<string, ClassDeclarationSyntax>();
      Init();
    }
    private void Init() {
      Deboss = new DefaultDeboss();
      Forme = new DefaultForme();
    }
    public IDeboss Deboss { get; set; }
    public IForme Forme { get; set; }

    /// <summary>
    /// Loads c# classes that to be processed and generate file.
    /// </summary>
    /// <param name="directoryPath">Filesystem path to classes used to processes templates.</param>
    /// <returns>A dictionary containing the class name as key and the ClassDeclarationSyntax for the class as the value.</returns>
    public Dictionary<string, ClassDeclarationSyntax> LoadClasses(string directoryPath) {
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

    /// <summary>
    /// Debosses classes into individual files named by class placing them into specified 
    /// directory with the desired extension.
    /// </summary>
    /// <param name="outputDir">Where to output files</param>
    /// <param name="extension">Extension supplied to each file created.</param>
    public void Ink(string outputDir, string extension) {
      Deboss.Forme = Forme;
      foreach (var cls in Classes) {
        if (!Deboss.IncludeClass(cls.Value)) continue;
        string filename = cls.Key;
        string filePath = Path.Combine(outputDir, filename + extension);
        File.WriteAllText(filePath, Forme.GeneratedTemplate + Deboss.Impress(cls.Value));
      }
    }

    /// <summary>
    /// Debosses classes into individual files named by class placing them into specified 
    /// directory using a delagate function to allow for custom formatting based on ClassDeclarationSyntax.
    /// </summary>
    /// <param name="outputDir">Where to output files</param>
    /// <param name="fileName">Delegate function providing file name based on ClassDeclarationSyntax</param>
    public void Ink(string outputDir, Func<ClassDeclarationSyntax, string> fileName) {
      Deboss.Forme = Forme;
      foreach (var cls in Classes) {
        if (!Deboss.IncludeClass(cls.Value)) continue;
        string filePath = Path.Combine(outputDir, fileName(cls.Value));
        File.WriteAllText(filePath, Forme.GeneratedTemplate + Deboss.Impress(cls.Value));
      }
    }

    /// <summary>
    /// Places all templated classes into a single file.
    /// </summary>
    /// <param name="outputDir">Where to output files</param>
    /// <param name="fileName">Name of file to place all classes into</param>
    public void InkToFile(string outputDir, string fileName) {
      Deboss.Forme = Forme;
      string filePath = Path.Combine(outputDir, fileName);
      File.WriteAllText(filePath, "");// Recreate file.
      var classStrBuilder = new StringBuilder();
      foreach (var cls in Classes) {
        if (!Deboss.IncludeClass(cls.Value)) continue;
        classStrBuilder.Append(Deboss.Impress(cls.Value));
      }
      File.WriteAllText(filePath, Forme.GeneratedTemplate + Forme.AllTemplate.Replace("${All}", classStrBuilder.ToString()));
    }
  }
}
