using LetterPress.Extensions;
using LetterPress.Forme;
using LetterPress.Replacements;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LetterPress.Deboss
{
  public class DefaultDeboss : IDeboss {
    public virtual IForme Forme { get; set; }
    public virtual Func<ClassDeclarationSyntax, bool> IncludeClass { get; set; }
    public virtual Func<PropertyDeclarationSyntax, bool> IncludeProperty { get; set; } = (_) => true;
    public virtual Func<MethodDeclarationSyntax, bool> IncludeMethod { get; set; }

    public DefaultDeboss() {
    }

    public virtual string Impress(Dictionary<string, ClassDeclarationSyntax> classes) {
      var str = "";
      foreach (var cls in classes.Values) {
        if (!IncludeClass(cls)) continue;
        str += Impress(cls);
      }
      return str;
    }

    public string Impress(ClassDeclarationSyntax cls, string template) {

      if (IncludeClass != null && !IncludeClass(cls)) return "";
      Forme.ClassTemplate = template;

      return Impress(cls);
    }

    public virtual string Impress(ClassDeclarationSyntax cls, string template, bool? preserveDefaultTemplate) {
      string preserve = "";
      if (Forme == null) Forme = new DefaultForme();
      if (IncludeClass != null && !IncludeClass(cls)) return "";
      if (!preserveDefaultTemplate ?? false) Forme.ClassTemplate = template;
      return ImpressTemplate(cls, template);
    }
    public virtual string Impress(ClassDeclarationSyntax cls) =>
      (IncludeClass != null && !IncludeClass(cls)) ? "" :
      ImpressTemplate(cls, Forme.ClassTemplate);

    private string ImpressTemplate(ClassDeclarationSyntax cls, string template) =>
      template     
         .Replace(Clazz.ClassName, cls.Identifier.Text)
         .Replace(
            Clazz.TableName,
            cls.GetFirstAttrProperty("Table")?.Trim('"') ?? cls.Identifier.Text
          )
          .Replace(
            Clazz.ClassAsArguement,
            cls.name()
          )
          .Replace(
             Clazz.Generics,
             cls?.TypeParameterList?.ToString() ?? ""
          )
         .Replace(Clazz.Properties,
            string.Join(
              "\n",
              cls.Members.OfType<PropertyDeclarationSyntax>()
                .ToList()
                .Where(p => IncludeProperty(p))
                .Select(p =>
                  Impress(p)
                )
            )
          )
          .Replace(Clazz.Methods,
             string.Join(
               "\n",
               cls.Members.OfType<MethodDeclarationSyntax>()
               .ToList()
               .Select(m =>
                  Impress(m)
               )
             )

          );



    public string Impress(PropertyDeclarationSyntax prop, string template) {
      Forme.PropertyTemplate = template;
      return Impress(prop);
    }

    public virtual string Impress(PropertyDeclarationSyntax prop) =>
      Forme.PropertyTemplate
        .Replace(Property.PropertyName, prop.Identifier.Text)
        .Replace(Property.PropertyType, prop.Type.ToString());

    public virtual string Impress(MethodDeclarationSyntax method, string template) {
      Forme.MethodTemplate = template;
      return Impress(method);
    }

    public virtual string Impress(MethodDeclarationSyntax method) =>
      Forme.MethodTemplate
      .Replace(Method.MethodName, method.Identifier.Text)
      .Replace(Method.ReturnType, method.ReturnType.ToString())
      .Replace(Method.Parameters, 
        string.Join(",",
          method.ParameterList.Parameters
          .ToList()
          .Select(p =>
            Impress(p)
          )
        )
      );
    public string Impress(ParameterSyntax param, string template) {
      Forme.ParameterTemplate = template;
      return Impress(param);
    }
    public virtual string Impress(ParameterSyntax param) {
      return Forme.ParameterTemplate
        .Replace(Parameter.VariableName, param.Identifier.Text)
        .Replace(Parameter.VariableType, param.Type.ToString());
    }
  }
}

