using LetterPress.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterPress.Examples.Entities {
  public class Widget {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [NoTypeScript]
    public Guid SmidgetId { get; set; }
    [NoTypeScript]
    public Smidget Smidget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    [NoTypeScript]
    public bool DeletedAt { get; set; }
  }
}
