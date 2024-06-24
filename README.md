# LetterPress

### Overview

LetterPress is a library inspired by the Typewriter Visual Studio plugin, designed to streamline and enhance the development process by leveraging T4 templates for advanced code generation. By extending the capabilities of EntityFramework, LetterPress facilitates the automatic creation of clientside TypeScript code for Services and Models, significantly reducing manual coding efforts and minimizing errors.

### Features

- **Code Generation with T4 Templates**: Utilize T4 templates to generate TypeScript code based on C# classes, ensuring consistency and reducing redundancy in your project.
  
- **Enhanced EntityFramework Integration**: Expand on the standard ORM database migrations provided by EntityFramework to automatically generate clientside TypeScript code for Services and Models.

- **Flexible Controller Generation**: Automatically generate Controllers with customizable implementations:
  - **Full Implementations**: Create complete Controller classes based on your C# code.
  - **Partial Implementations**: Generate basic implementations with placeholders for custom action methods, allowing for easy extension and customization.
  
- **Attribute-Based Control**: Use C# attributes to control the generation process:
  - **[NoTypeScript]**: Omit specific classes, properties, or methods from the TypeScript code generation.
  - **[NoHttpPut, NoHttpGet, NoHttpPost, NoHttpDelete]**(coming): Exclude elements from specific operations, providing fine-grained control over what gets generated.

### Benefits

- **Consistency**: Ensure consistent structure and naming conventions across your clientside and serverside code.
- **Efficiency**: Save development time by automating repetitive coding tasks and reducing manual intervention.
- **Customization**: Easily tailor the generated code to fit specific project requirements through attribute-based exclusions.
- **Seamless Integration**: Integrate seamlessly with existing EntityFramework workflows, making it easy to adopt without major changes to your current setup.

### Getting Started

To start using LetterPress in your project, follow these steps:

1. **Define Your C# Classes**: Create your EntityFramework models as usual.
2. **Apply Attributes**: Use the provided attributes ([NoTypeScript], [NoPut], etc.) to control the code generation process.
3. **Run the Generator**: Execute the T4 template to generate the TypeScript code for your Services, Models, and Controllers.

### Conclusion

LetterPress is designed to simplify and enhance the development process by providing robust tools for code generation. By automating repetitive tasks and ensuring consistency across your codebase, it allows developers to focus on creating value-added features and improving overall productivity.

---

Feel free to contribute to the project or report issues on our [GitHub repository](https://github.com/spillcoffee/LetterPress). Together, we can make development more efficient and enjoyable with LetterPress.
