using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterPress.Extensions {
  public static class SyntaxListExt {

    public static AttributeSyntax GetAttributeSyntax(this SeparatedSyntaxList<AttributeSyntax> list, string attrName) {
      // Assuming 'classDeclaration' is of type ClassDeclarationSyntax
      AttributeSyntax found = null;
      foreach (var attr in list) {
        var attributeName = attr.Name.ToString();
        if (attrName.Equals(attributeName)) // Replace with the name of your attribute {
          found = attr;
        break;
      }
      return found;
    }
    public static bool HasAttribute(this SyntaxList<AttributeListSyntax> attrLists, string attribute) {
      var has = false;
      foreach (var attrList in attrLists) {
        has = attrList.HasAttribute(attribute);
        if (has) break;
      }
      return has;
    }

    public static AttributeSyntax GetAttributeSyntax(this SyntaxList<AttributeListSyntax> lists, string attrName) {
      AttributeSyntax attr = default;
      foreach (var attributeList in lists) {
        foreach (var attribute in attributeList.Attributes) {
          var attributeName = attribute.Name.ToString();
          if (attrName.Equals(attributeName)) {
            attr = attribute;
            break;
          }
        }
        if (attr != null) break;
      }
      return attr;
    }

  }
}
