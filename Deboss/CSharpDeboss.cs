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
  public class CSharpDeboss : DefaultDeboss {

    public Func<ClassDeclarationSyntax, string> KeysAsParamsPrefix { get; set; } = _ => string.Empty;
    public Func<ClassDeclarationSyntax, string> AName { get; set; } = null;
    public Func<ClassDeclarationSyntax, string> BName { get; set; } = null;

    private string NewGuidKeysForA(ClassDeclarationSyntax cls) =>
        string.Join("\n", cls
        .Members
        .OfType<PropertyDeclarationSyntax>()
        .Where(p => p.AttributeLists.HasAttribute("Key"))
        .ToList()
        .Select(p => Impress(p, $"{AName(cls)}.{Property.PropertyName} = Guid.NewGuid();"))
        );

    private string CopyAToBParams(ClassDeclarationSyntax cls) {
      if (AName == null || BName == null) return "// Supply CopyAName and CopyBName to Deboss.";
      return string.Join("\n", cls
        .Members
        .OfType<PropertyDeclarationSyntax>()
        .Where(p => !p.AttributeLists.HasAttribute("Key") && !p.AttributeLists.HasAttribute("NoCopy"))
        .ToList()
        .Select(p => Impress(p, $"{AName(cls)}.{Property.PropertyName} = {BName(cls)}.{Property.PropertyName};"))
        );
    }

    private string KeysAsPath(ClassDeclarationSyntax cls) =>
      "/" + string.Join("/", cls
        .Members
        .OfType<PropertyDeclarationSyntax>()
        .Where(p => p.AttributeLists.HasAttribute("Key"))
        .ToList()
        .Select(p => Impress(p, $":{Property.CSharpPropAsParam}"))
        );
    private string KeysAsParams(ClassDeclarationSyntax cls) =>
      string.Join(",", cls
        .Members
        .OfType<PropertyDeclarationSyntax>()
        .Where(p => p.AttributeLists.HasAttribute("Key"))
        .ToList()
        .Select(p => Impress(p, $"{KeysAsParamsPrefix(cls)}{Property.PropertyType} {Property.CSharpPropAsParam}"))
        );

    private string KeysXEqParams(ClassDeclarationSyntax cls) =>
      string.Join(" && ", cls
        .Members
        .OfType<PropertyDeclarationSyntax>()
        .Where(p => p.AttributeLists.HasAttribute("Key"))
        .ToList()
        .Select(p =>
           Impress(p, $"x.{Property.PropertyName}.Equals({Property.CSharpPropAsParam})"
             .Replace(Clazz.ClassAsArguement, cls.name())
             )
           )
        );
    private string KeysXEqClsAsParam(ClassDeclarationSyntax cls) =>
      string.Join(" && ", cls
        .Members
        .OfType<PropertyDeclarationSyntax>()
        .Where(p => p.AttributeLists.HasAttribute("Key"))
        .ToList()
        .Select(p =>
           Impress(p, $"x.{Property.PropertyName}.Equals({Clazz.ClassAsArguement}.{Property.PropertyName})"
             .Replace(Clazz.ClassAsArguement, cls.name())
             )
           )
        );

    public override string Impress(ClassDeclarationSyntax cls, string template, bool? preserveDefaultTemplate) =>
      base.Impress(cls, template, preserveDefaultTemplate)
        .Replace(Clazz.ClassAsArguement, cls.name())
        .Replace(Clazz.KeysAsUrl, KeysAsPath(cls))
        .Replace(Clazz.KeysAsParams, KeysAsParams(cls))
        .Replace(Clazz.KeysXEqClsAsParam, KeysXEqClsAsParam(cls))
        .Replace(Clazz.KeysXEqParams, KeysXEqParams(cls))
        .Replace(Clazz.CopyProperties_AToB, CopyAToBParams(cls))
        .Replace(Clazz.NewGuidKeys_A, NewGuidKeysForA(cls));
    public override string Impress(ClassDeclarationSyntax cls) =>
      base.Impress(cls)
        .Replace(Clazz.ClassAsArguement, cls.name())
        .Replace(Clazz.KeysAsUrl, KeysAsPath(cls))
        .Replace(Clazz.KeysAsParams, KeysAsParams(cls))
        .Replace(Clazz.KeysXEqClsAsParam, KeysXEqClsAsParam(cls))
        .Replace(Clazz.KeysXEqParams, KeysXEqParams(cls))
        .Replace(Clazz.CopyProperties_AToB, CopyAToBParams(cls))
        .Replace(Clazz.NewGuidKeys_A, NewGuidKeysForA(cls));

    public override string Impress(PropertyDeclarationSyntax prop) =>
      base.Impress(prop)
      .Replace(Property.CSharpPropAsParam, prop.name())
      ;
  }
}
