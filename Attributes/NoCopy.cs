using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Attributes {
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class NoCopy : Attribute {
  }
}
