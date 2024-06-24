using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetterPress.Extensions {
  public static class ClassDeclarationExt {

    public static string Name(this ClassDeclarationSyntax cls) => cls.Identifier.Text.ToString();

    public static string name(this ClassDeclarationSyntax cls) =>
      new string(cls.Identifier.Text.Select((c, i) => i == 0 ? char.ToLower(c) : c).ToArray());

    public static bool HasAttribute(this ClassDeclarationSyntax cls, string attribute) {
      var has = false;
      foreach (var attrList in cls.AttributeLists) {
        has = attrList.HasAttribute(attribute);
        if (has) break;
      }
      return has;
    }

    public static AttributeSyntax GetAttributeSyantax(this ClassDeclarationSyntax cls, string findAttributeName) {
      AttributeSyntax attr = null;
      // Assuming 'classDeclaration' is of type ClassDeclarationSyntax
      foreach (var attributeList in cls.AttributeLists) {
        attr = attributeList.Attributes.GetAttributeSyntax(findAttributeName);
        if (attr != null) break;
      }
      return attr;
    }

    public static string GetFirstAttrProperty(this ClassDeclarationSyntax cls, string findAttributeName) {
      AttributeSyntax attr = GetAttributeSyantax(cls, findAttributeName);
      if (attr != null && (attr.ArgumentList?.Arguments.Any() ?? false)) {
        return attr.ArgumentList.Arguments.First().ToString();
      }
      return null;
    }
  }
}
