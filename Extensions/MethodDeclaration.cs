using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Extensions {
  public static class MethodDeclaration {
    public static bool HasAttribute(this MethodDeclarationSyntax method, string attribute) {
      var has = false;
      foreach (var attrList in method.AttributeLists) {
        has = attrList.HasAttribute(attribute);
        if (has) break;
      }
      return has;
    }

  }
}
