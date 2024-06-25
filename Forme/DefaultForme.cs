using LetterPress.Replacements;
using System;

namespace LetterPress.Forme {
  public class DefaultForme : IForme {

    public DefaultForme() {
      AllTemplate = "${All}";
      ClassTemplate = $"Default impressing of class {Clazz.ClassName}\n" + Clazz.Properties + "\n";
      GeneratedTemplate = $"/*\n * Generated code, do not modify!\n */\n\n";
      MethodTemplate = $"Default impressing of method {Method.ReturnType} {Method.MethodName}\n";
      ParameterTemplate = $"- Default impressing of parameter {Parameter.VariableType} {Parameter.VariableName}\n";
      PropertyTemplate = $"-.Default impressing of property {Property.PropertyType} {Property.PropertyName}\n";
    }
    public string AllTemplate { get; set; }
    public string ClassTemplate { get; set; }
    public string GeneratedTemplate { get; set; }
    public string MethodTemplate { get; set; }
    public string ParameterTemplate { get; set; }
    public string PropertyTemplate { get; set; }
  }
}
