using LetterPress.Extensions;
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
    public Func<ClassDeclarationSyntax, string> NewableGuidKeysVariableName { get; set; } = _ => string.Empty;

    private string ClsNewableGuidKeys(ClassDeclarationSyntax cls) =>
      cls.BaseList?.Types
                   .Any(baseType => baseType.ToString() == "INewableGuidKeys") == true
      && cls
      .Members
      .OfType<PropertyDeclarationSyntax>()
      .Where(p => p.AttributeLists.HasAttribute("Key") && p.Type.ToString().Equals("Guid"))
      .Any() ? $"{NewableGuidKeysVariableName(cls)}.NewGuidKeys();" : "";


    private string KeysAsPath(ClassDeclarationSyntax cls)  => 
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
             .Replace(Clazz.CSharpClsAsParam, cls.name())
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
           Impress(p, $"x.{Property.PropertyName}.Equals({Clazz.CSharpClsAsParam}.{Property.PropertyName})"
             .Replace(Clazz.CSharpClsAsParam, cls.name())
             )
           )
        );


    public override string Impress(ClassDeclarationSyntax cls) {
      return base.Impress(cls)
        .Replace(Clazz.CSharpClsAsParam, cls.name())
        .Replace(Clazz.KeysAsUrl, KeysAsPath(cls))
        .Replace(Clazz.KeysAsParams, KeysAsParams(cls))
        .Replace(Clazz.KeysXEqClsAsParam, KeysXEqClsAsParam(cls))
        .Replace(Clazz.KeysXEqParams, KeysXEqParams(cls))
        .Replace(Clazz.CSharpNewableGuidKeys, ClsNewableGuidKeys(cls));
    }
    public override string Impress(PropertyDeclarationSyntax prop) =>
      base.Impress(prop)
      .Replace(Property.CSharpPropAsParam, prop.name())
      ;
  }
}
