using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Forme {
  /// <summary>
  /// In terms of a Letter Press a Forme is as follows:
  /// A forme is a collection of types and spacing materials 
  /// locked into a chase (a metal frame) to create a page layout.
  /// 
  /// In this project a Forme is a collection of templates, used by the Press
  /// </summary>
  public interface IForme {
    string AllTemplate { get; set; }
    string ClassTemplate { get; set; }
    string GeneratedTemplate { get; set; }
    string MethodTemplate { get; set; }
    string ParameterTemplate { get; set; }
    string PropertyTemplate { get; set; }
  }
}
