using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterPress.Extensions {
  public static class AttributeSyntaxList {
    public static bool HasAttribute(this AttributeListSyntax attrList, string attribute) {
      var has = false;
      foreach (var attr in attrList.Attributes) {
        if (attr.Name.ToString().Equals(attribute)) {
          has = true;
          break;
        }
      }
      return has;
    }
  }
}
