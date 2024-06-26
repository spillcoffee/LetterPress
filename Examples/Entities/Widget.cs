using LetterPress.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Examples.Entities {
  [Table("Widgets")]
  public class Widget {
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [NoTypeScript, NoCopy]
    public Guid SmidgetId { get; set; }
    [NoTypeScript, NoCopy]
    public Smidget Smidget { get; set; }
    [NoCopy]
    public DateTime CreatedAt { get; set; }
    [NoCopy]
    public DateTime LastUpdatedAt { get; set; }
    [NoTypeScript, NoCopy]
    public bool DeletedAt { get; set; }
  }
}
