using LetterPress.Forme;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Runtime.Remoting.Contexts;

namespace LetterPress.Deboss {
  /// <summary>
  /// Deboss refers to a printing technique in which an image, design, or text is 
  /// pressed into a material (like paper, leather, or fabric) so that it creates 
  /// a recessed, sunken effect. This is achieved using a metal plate or die, which 
  /// is pushed into the material with sufficient pressure, creating an indentation. 
  /// Debossing is often used for decorative purposes on book covers, invitations, 
  /// business cards, and other printed materials to give a tactile and visually 
  /// appealing finish.
  /// 
  /// In the context of this project, "Deboss" is metaphorically to represent 
  /// foundational classes or components that provide the underlying structure and 
  /// logic for your code generation process, much like how debossing provides a 
  /// subtle, recessed imprint.
  /// </summary>
  public interface IDeboss {
    IForme Forme { get; set; }
    string Impress(ClassDeclarationSyntax cls);
    string Impress(ClassDeclarationSyntax cls, string template);
    string Impress(ClassDeclarationSyntax cls, string template, bool? preserveDefaultTemplate);
    string Impress(MethodDeclarationSyntax method);
    string Impress(MethodDeclarationSyntax method, string template);
    string Impress(ParameterSyntax param);
    string Impress(ParameterSyntax param, string template);
    string Impress(PropertyDeclarationSyntax prop);
    string Impress(PropertyDeclarationSyntax prop, string template);
    Func<ClassDeclarationSyntax, bool> IncludeClass { get; set; }
    Func<PropertyDeclarationSyntax, bool> IncludeProperty { get; set; }
    Func<MethodDeclarationSyntax, bool> IncludeMethod { get; set; }
  }
}