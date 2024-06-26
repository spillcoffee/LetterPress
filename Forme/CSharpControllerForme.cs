using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Forme {
  public class CSharpControllerForme : FileLoadForme, IControllerForme {
    public string HttpGetTemplate { get; set; } = string.Empty;
    public string HttpPostTemplate { get; set; } = string.Empty;
    public string HttpPutTemplate { get; set; } = string.Empty;
    public string HttpDeleteTemplate { get; set; } = string.Empty;
    public void LoadHttpGetTemplate(string fileLocation) => Load("HttpGetTemplate", fileLocation);
    public void LoadHttpPostTemplate(string fileLocation) => Load("HttpPostTemplate", fileLocation);
    public void LoadHttpPutTemplate(string fileLocation) => Load("HttpPutTemplate", fileLocation);
    public void LoadHttpDeleteTemplate(string fileLocation) => Load("HttpDeleteTemplate", fileLocation);
  }
}
