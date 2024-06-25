using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Forme {
  public class FileLoadForme : DefaultForme {
    public void LoadAllTemplate(string fileLocation) => Load("AllTemplate", fileLocation);
    public void LoadClassTemplate(string fileLocation) => Load("ClassTemplate", fileLocation);
    public void LoadGeneratedTemplate(string fileLocation) => Load("GeneratedTemplate", fileLocation);
    public void LoadMethodTemplate(string fileLocation) => Load("MethodTemplate", fileLocation);
    public void LoadParameterTemplate(string fileLocation) => Load("ParameterTemplate", fileLocation);
    public void LoadPropertyTemplate(string fileLocation) => Load("PropertyTemplate", fileLocation);
    public void Load(string propertyName, string fileLocation) {
      PropertyInfo property = this
        .GetType()
        .GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      if (property != null && property.PropertyType == typeof(string)) {
        string content = File.ReadAllText(fileLocation);
        property.SetValue(this, content);
      } else {
        throw new ArgumentException($"Property '{propertyName}' not found or is not of type string.");
      }
    }
  }
}

