using LetterPress.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Examples.Entities {
  [NoTypeScript]
  public class Smidget {
    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}
