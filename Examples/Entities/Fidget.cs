using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Examples.Entities {
  [Table("Fidgets"), NoHttpDelete]
  public class Fidget {
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}
