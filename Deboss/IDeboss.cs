using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace LetterPress.Deboss {
  public interface IDeboss {
    string ClassTemplate { get; set; }
    string MethodTemplate { get; set; }
    string PropertyTemplate { get; set; }
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
  }
}