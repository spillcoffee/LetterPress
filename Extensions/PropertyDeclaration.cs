using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetterPress.Extensions {
  public static class PropertyDeclaration {
    public static string Name(this PropertyDeclarationSyntax prop) => prop.Identifier.Text.ToLower();
    public static string name(this PropertyDeclarationSyntax prop) =>
      new string(prop.Identifier.Text.Select((c, i) => i == 0 ? char.ToLower(c) : c).ToArray());
    public static bool HasAttribute(this PropertyDeclarationSyntax prop, string attribute) {
      var has = false;
      foreach (var attrList in prop.AttributeLists) {
        has = attrList.HasAttribute(attribute);
        if (has) break;
      }
      return has;
    }

  }
}
