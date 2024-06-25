# LetterPress

## Overview

LetterPress is a library inspired by the Typewriter Visual Studio plugin, designed to streamline and enhance the development process by leveraging T4 templates for advanced code generation. By extending the capabilities of EntityFramework, LetterPress facilitates the automatic creation of clientside TypeScript code for Services and Models, significantly reducing manual coding efforts and minimizing errors.

## Features

- **Code Generation with T4 Templates**: Utilize T4 templates to generate TypeScript code based on C# classes, ensuring consistency and reducing redundancy in your project.
- **Enhanced EntityFramework Integration**: Expand on the standard ORM database migrations provided by EntityFramework to automatically generate clientside TypeScript code for Services and Models.
- **Flexible Controller Generation**: Automatically generate Controllers with customizable implementations:
  - **Full Implementations**: Create complete Controller classes based on your C# code.
  - **Partial Implementations**: Generate basic implementations with placeholders for custom action methods, allowing for easy extension and customization.
- **Attribute-Based Control**: Use C# attributes to control the generation process:
  - **[NoTypeScript]**: Omit specific classes, properties, or methods from the TypeScript code generation.

## Benefits

- **Consistency**: Ensure consistent structure and naming conventions across your clientside and serverside code.
- **Efficiency**: Save development time by automating repetitive coding tasks and reducing manual intervention.
- **Customization**: Easily tailor the generated code to fit specific project requirements through attribute-based exclusions.
- **Seamless Integration**: Integrate seamlessly with existing EntityFramework workflows, making it easy to adopt without major changes to your current setup.

## Getting Started

To start using LetterPress in your project, follow these steps:

1. Define Your C# Classes: Create your EntityFramework models as usual.
2. Apply Attributes: Use the provided attributes ([NoTypeScript], [NoPut], etc.) to control the code generation process.
3. Run the Generator: Execute the T4 template to generate the TypeScript code for your Services, Models, and Controllers.

### Example C# Classes

```csharp
public class Fidget {
  public Guid	Id { get; set; }
  public string Name { get; set; }
}

[NoTypeScript]
public class Smidget {
  public Guid Id { get; set; }
  public string Name { get; set; }
}

public class Widget
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [NoTypeScript]
    public Guid SmidgetId { get; set; }

    [NoTypeScript]
    public Smidget Smidget { get; set; }
}
```

### Example TypeScript templates
```typescript
// File: ts_class.template
export interface I${ClassName} {
${Properties}
}

export class ${ClassName} implements I${ClassName} {
  state: I${ClassName}
${Properties}
  constructor(raw: I${ClassName}) {
    for (let key in Object.keys(raw)) {
      this[key] = raw[key];
    }
    this.state = raw;    
  }
}
// File: ts_property.template
    ${PropertyName}: ${PropertyType};
```
### Can be incorporated into a .tt or .t4 template file
Given the appropriate imports are inplace this can be added to a 
t4 template to be called on build of a csproj, or during development in
Visual Studio.  Examples of imports are in the /Examples directory using 
t4 templates.
```csharp
string currentDirectory = Directory.GetCurrentDirectory();
var press = new Press();
press.LoadClasses($@"{currentDirectory}/Examples/Entities");

// Configure deboss
var tsDeboss = new TypescriptDeboss();
tsDeboss.ReferenceClasses = press.Classes;
press.Deboss = tsDeboss;

// Configure templates by specifying a IForme instance.
var forme = new FileLoadForme();
forme.LoadGeneratedTemplate(
  this.Host.ResolvePath($@"{currentDirectory}/Examples/generated.template")
);
forme.LoadClassTemplate(
  this.Host.ResolvePath($@"{currentDirectory}/Examples/NoTypeScript/ts_class.template")
);
forme.LoadPropertyTemplate(
  this.Host.ResolvePath($@"{currentDirectory}/Examples/NoTypeScript/ts_property.template")
);
press.Forme = forme;

// Generate code by "Inking" them.
press.Ink(
  $@"{currentDirectory}/Examples/NoTypeScript/Output", // Output of deboss
  (cls) => $"{press.Deboss.Impress(cls, "${ClassName}", true)}.g.ts" // Filename to deboss into
  );

```

### Output into individual files
```typescript
// Generated file: Fidget.g.ts
export interface IFidget {
    Id: string;
    Name: string;

}

export class Fidget implements IFidget {
  state: IFidget
    Id: string;
    Name: string;

  constructor(raw: IFidget) {
    for (let key in Object.keys(raw)) {
      this[key] = raw[key];
    }
    this.state = raw;    
  }
}
// Skipped generating file: Smidget.g.ts
// Generated file: Widget.g.ts
export interface IWidget {
    Id: string;
    Name: string;
    Description: string;
    CreatedAt: Date;
    LastUpdatedAt: Date;

}

export class Widget implements IWidget {
  state: IWidget
    Id: string;
    Name: string;
    Description: string;
    CreatedAt: Date;
    LastUpdatedAt: Date;

  constructor(raw: IWidget) {
    for (let key in Object.keys(raw)) {
      this[key] = raw[key];
    }
    this.state = raw;    
  }
}

```
### Output all classes to a single file
Change configuration of Forme and Press
```csharp
// Specify a file to used for placement of generated classes, in this case
// used to provide any needed imports for typescript.
forme.LoadAllTemplate(
  this.Host.ResolvePath($@"{currentDirectory}/Examples/NoTypeScriptOneFile/ts_all.template")
);

// Call InkToFile instead
press.InkToFile(
  $@"{currentDirectory}/Examples/NoTypeScriptOneFile/Output", // Output of deboss
  "ALL.g.ts" // Place all files into one file,
  );
```
Inking creates a single file All*.**g*ts
```typescript
import { Datum } from "./Datum";

export interface IFidget {
  Id: string;
  Name: string;

}

export class Fidget extends Datum<IFidget> {
  state: IFidget
  Id: string;
  Name: string;

  constructor(raw: IWidget) {
    super();
    for (let key in Object.keys(raw)) {
      this[key] = raw[key];
    }
    this.state = raw;    
  }
}
export interface IWidget {
  Id: string;
  Name: string;
  Description: string;
  CreatedAt: Date;
  LastUpdatedAt: Date;

}

export class Widget extends Datum<IWidget> {
  state: IWidget
  Id: string;
  Name: string;
  Description: string;
  CreatedAt: Date;
  LastUpdatedAt: Date;

  constructor(raw: IWidget) {
    super();
    for (let key in Object.keys(raw)) {
      this[key] = raw[key];
    }
    this.state = raw;    
  }
}

```