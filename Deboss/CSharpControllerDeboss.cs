using LetterPress.Extensions;
using LetterPress.Forme;
using LetterPress.Replacements;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Deboss {
  public class CSharpControllerDeboss<F> : CSharpDeboss where F : IForme, IControllerForme {

    private F _forme;
    public override IForme Forme {
      get => _forme;
      set {
        if (value is F) {
          _forme = (F)value;
        } else {
          throw new InvalidOperationException("Forme must be of type IForme and IControllerForme");
        }
      }
    }
    private string HttpGetSection(ClassDeclarationSyntax cls) {
      if (cls.AttributeLists.HasAttribute("NoHttpGet")) return string.Empty;
      return _forme.HttpGetTemplate;
    }
    private string HttpPostSection(ClassDeclarationSyntax cls) {
      if (cls.AttributeLists.HasAttribute("NoHttpPost")) return string.Empty;
      return _forme.HttpPostTemplate;
    }
    private string HttpPutSection(ClassDeclarationSyntax cls) {
      if (cls.AttributeLists.HasAttribute("NoHttpPut")) return string.Empty;
      return _forme.HttpPutTemplate;
    }
    private string HttpDeleteSection(ClassDeclarationSyntax cls) {
      if (cls.AttributeLists.HasAttribute("NoHttpDelete")) return string.Empty;
      return _forme.HttpDeleteTemplate;
    }
    public override string Impress(ClassDeclarationSyntax cls) {
      var template = Forme.ClassTemplate
        .Replace(CSharpController.HttpGet, HttpGetSection(cls))
        .Replace(CSharpController.HttpPost, HttpPostSection(cls))
        .Replace(CSharpController.HttpPut, HttpPutSection(cls))
        .Replace(CSharpController.HttpDelete, HttpDeleteSection(cls));
      return base.Impress(cls, template, true);
    }
  }
  public class CSharpControllerDeboss : CSharpControllerDeboss<CSharpControllerForme> {
    public CSharpControllerDeboss() : base() { }
  }
}
