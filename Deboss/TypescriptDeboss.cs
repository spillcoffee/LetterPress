using LetterPress.Attributes;
using LetterPress.Extensions;
using LetterPress.Replacements;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetterPress.Deboss {
  public class TypescriptDeboss : DefaultDeboss {

    public Dictionary<string, ClassDeclarationSyntax> Classes { get; set; }
    public override Func<ClassDeclarationSyntax, bool> IncludeClass { get; set; } = 
        (cls) => !cls.HasAttribute(typeof(NoTypeScript).Name);
    public override Func<PropertyDeclarationSyntax, bool> IncludeProperty { get; set; } =
        (prop) => !prop.HasAttribute(typeof(NoTypeScript).Name);
    public override Func<MethodDeclarationSyntax, bool> IncludeMethod { get; set; } =
        (method) => !method.HasAttribute(typeof(NoTypeScript).Name);
    public override string Impress(ClassDeclarationSyntax cls) => !IncludeClass(cls)
      ? ""
      : ClassTemplate
        .Replace(Clazz.ClassName, cls.Identifier.Text)
        .Replace(Clazz.Properties,
           string.Join("",
             cls
             .Members.OfType<PropertyDeclarationSyntax>()
             .ToList()
             .Select(p => Impress(p))
             )
           )
        .Replace(Clazz.Methods, 
          string.Join("",
            cls
            .Members.OfType<MethodDeclarationSyntax>()
            .ToList()
            .Select(m => Impress(m)
            )
          )
        );
    public override string Impress(PropertyDeclarationSyntax prop) => !IncludeProperty(prop)
      ? ""
      : PropertyTemplate
        .Replace(Property.PropertyName, prop.Identifier.Text)
        .Replace(Property.PropertyType, TypeToTypecript(prop.Type, prop.Ancestors()));

    public override string Impress(MethodDeclarationSyntax method) => !IncludeMethod(method)
      ? ""
      : MethodTemplate
        .Replace(Method.MethodName, method.Identifier.Text)
        .Replace(Method.ReturnType, TypeToTypecript(method.ReturnType, method.Ancestors()))
        ;

    public override string Impress(ParameterSyntax param) =>
      ParameterTemplate
        .Replace(Parameter.VariableName, param.Identifier.Text)
        .Replace(Parameter.VariableType, TypeToTypecript(param.Type, param.Ancestors()))
        ;

    /*** Private helper methods follow ***/

    private static readonly string[] STRING_TYPES =
      new string[] { 

        "string", "string?", "String",  "String?", "Guid" };
    private static readonly string[] BOOL_TYPES =
      new string[] { "bool", "bool?", "Boolean", "Boolean?" };

    private static readonly string[] DATE_TYPES =
      new string[] {
        "DateTime", "DateTime?", "DateTimeOffset", "DateTimeOffset?"
      };

    private static readonly string[] NUMBER_TYPES =
      new string[] {
      "byte", "byte?", "sbyte", "sbyte?", "short", "short?", "ushort",
      "ushort?", "int", "int?", "uint", "uint?", "long", "long?", "ulong",
      "ulong?", "float", "float?", "double", "double?", "decimal", "decimal?"
    };

    private string TypescriptPrimitive(string type) {
      if (NUMBER_TYPES.Contains(type)) return "number";
      else if (BOOL_TYPES.Contains(type)) return "boolean";
      else if (DATE_TYPES.Contains(type)) return "Date";
      else if (STRING_TYPES.Contains(type)) return "string";
      return "unknown";
    }

    private string TypeToTypecript(TypeSyntax type, IEnumerable<SyntaxNode> ancestors) {
      //PropertyDeclarationSyntax prop
      if (Classes.ContainsKey(type.ToString())) {
        return Classes[type.ToString()].Identifier.Text;
      }

      if (type.ToString().StartsWith("List<")) {
        if (type is GenericNameSyntax genericType) {
          var innerType = genericType.TypeArgumentList.Arguments.FirstOrDefault();
          if (Classes.ContainsKey(innerType.ToString())) {
            return $"{Classes[innerType.ToString()].Identifier.Text}[]";
          }

          return $"{TypescriptPrimitive(innerType.ToString())}[]";
        }
      }

      // Now verify the type is not the generic of the parent class, before checking primitives.
      var cls = ancestors.OfType<ClassDeclarationSyntax>().FirstOrDefault();
      var typeParameters = cls.TypeParameterList?.Parameters.Select(p => p.Identifier.Text).ToList();
      if (typeParameters != null && typeParameters.Contains(type.ToString())) {
        // We just return it, it will work itself out.
        return type.ToString();
      }

      return TypescriptPrimitive(type.ToString());
    }
  }
}
