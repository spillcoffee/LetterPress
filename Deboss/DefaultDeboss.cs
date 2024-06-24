using LetterPress.Extensions;
using LetterPress.Replacements;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;

using System.Text;

namespace LetterPress.Deboss
{
  public class DefaultDeboss : IDeboss {
    public virtual Func<ClassDeclarationSyntax, bool> IncludeClass { get; set; }
    public virtual Func<PropertyDeclarationSyntax, bool> IncludeProperty { get; set; }
    public virtual Func<MethodDeclarationSyntax, bool> IncludeMethod { get; set; }

    public string AllTemplate { get; set; }
    public string ClassTemplate { get; set; }
    public string GeneratedTemplate { get; set; }
    public string MethodTemplate { get; set; }
    public string ParameterTemplate { get; set; }
    public string PropertyTemplate { get; set; }
    public DefaultDeboss() {
      AllTemplate = "${All}";
      ClassTemplate = $"Default impressing of class {Clazz.ClassName}\n" + Clazz.Properties + "\n";
      GeneratedTemplate = $"/*\n * Generated code, do not modify!\n */\n\n";
      MethodTemplate = $"Default impressing of method {Method.ReturnType} {Method.MethodName}\n";
      ParameterTemplate = $"- Default impressing of parameter {Parameter.VariableType} {Parameter.VariableName}\n";
      PropertyTemplate = $"-.Default impressing of property {Property.PropertyType} {Property.PropertyName}\n";
    }

    private string Load(string fileLocation) => File.ReadAllText(fileLocation);
    public string LoadAllTemplate(string fileLocation) => AllTemplate = Load(fileLocation);
    public string LoadClassTemplate(string fileLocation) => ClassTemplate = Load(fileLocation);
    public string LoadGeneratedTemplate(string fileLocation) => GeneratedTemplate = Load(fileLocation);
    public string LoadMethodTemplate(string fileLocation) => MethodTemplate = Load(fileLocation);
    public string LoadParameterTemplate(string fileLocation) => ParameterTemplate = Load(fileLocation);
    public string LoadPropertyTemplate(string fileLocation) => PropertyTemplate = Load(fileLocation);

    public virtual void ImpressInto(Dictionary<string, ClassDeclarationSyntax> classes, string outputDir,  Func<ClassDeclarationSyntax, string> fileName) {
      foreach (var cls in classes) {
        if (!IncludeClass(cls.Value)) continue;
        string filePath = Path.Combine(outputDir, fileName(cls.Value));
        File.WriteAllText(filePath, GeneratedTemplate + Impress(cls.Value));
      }
    }

    public virtual void ImpressAllToFile(Dictionary<string, ClassDeclarationSyntax> classes, string outputDir, string fileName) {
      string filePath = Path.Combine(outputDir, fileName);
      File.WriteAllText(filePath, "");// Recreate file.
      var classStrBuilder = new StringBuilder();
      foreach (var cls in classes) {
        if (!IncludeClass(cls.Value)) continue;
        classStrBuilder.Append(Impress(cls.Value));

      }

      File.WriteAllText(filePath, GeneratedTemplate + AllTemplate.Replace("${All}", classStrBuilder.ToString()));
    }

    public virtual void ImpressInto(Dictionary<string, ClassDeclarationSyntax> classes, string outputDir, string extension) {
      foreach (var cls in classes) {
        if (!IncludeClass(cls.Value)) continue;
        string filename = cls.Key;
        string filePath = Path.Combine(outputDir, filename + extension);
        File.WriteAllText(filePath, GeneratedTemplate + Impress(cls.Value));
      }
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

      if (!IncludeClass(cls)) return "";
      ClassTemplate = template;

      return Impress(cls);
    }

    public string Impress(ClassDeclarationSyntax cls, string template, bool? preserveDefaultTemplate) {
      string preserve = "";
      if (!IncludeClass(cls)) return "";
      if (preserveDefaultTemplate ?? false) preserve = ClassTemplate;

      ClassTemplate = template;

      if (preserve.Length > 0) {
        var result = Impress(cls);
        ClassTemplate = preserve;
        return result;
      }

      return Impress(cls);
    }

    public virtual string Impress(ClassDeclarationSyntax cls) =>
      (!IncludeClass(cls)) ? "" :
      ClassTemplate
         .Replace(Clazz.ClassName, cls.Identifier.Text)
         .Replace(
            Clazz.TableName,
            cls.GetFirstAttrProperty("Table")?.Trim('"') ?? cls.Identifier.Text
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
      PropertyTemplate = template;
      return Impress(prop);
    }

    public virtual string Impress(PropertyDeclarationSyntax prop) =>
      PropertyTemplate
        .Replace(Property.PropertyName, prop.Identifier.Text)
        .Replace(Property.PropertyType, prop.Type.ToString());

    public virtual string Impress(MethodDeclarationSyntax method, string template) {
      MethodTemplate = template;
      return Impress(method);
    }

    public virtual string Impress(MethodDeclarationSyntax method) =>
      MethodTemplate
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
      ParameterTemplate = template;
      return Impress(param);
    }
    public virtual string Impress(ParameterSyntax param) {
      return ParameterTemplate
        .Replace(Parameter.VariableName, param.Identifier.Text)
        .Replace(Parameter.VariableType, param.Type.ToString());
    }
  }
}

