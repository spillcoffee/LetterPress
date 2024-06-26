using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Forme {
  public interface IControllerForme {
    string HttpGetTemplate { get; set; }
    string HttpPostTemplate { get; set; }
    string HttpPutTemplate { get; set; }
    string HttpDeleteTemplate { get; set; }

  }
}
