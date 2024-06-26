using LetterPress.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Examples.Entities {
  [Table("Smidgets"), NoTypeScript, NoHttpPost, NoHttpPut, NoHttpDelete]
  public class Smidget {
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

  }
}
