using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterPress.Extensions {
  public static class AttributeSyntaxExt {
    public static string GetFirstAttrProperty(this AttributeSyntax attr) {
      if (attr != null && (attr.ArgumentList?.Arguments.Any() ?? false)) {
        return attr.ArgumentList.Arguments.First().ToString().Trim('"');
      }
      return "";
    }
  }
}
