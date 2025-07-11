using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GeneratorLibrary
{
    [Generator]
    public class MainGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            Console.WriteLine($"Found  service classes.");
        }

        public void Execute(GeneratorExecutionContext context)
        {
            //             // We’ll look for classes named "WeatherService"
            //             var weatherServiceSymbol = context
            //                 .Compilation.GetSymbolsWithName("WeatherService", SymbolFilter.Type)
            //                 .OfType<INamedTypeSymbol>()
            //                 .FirstOrDefault();
            //
            //             if (weatherServiceSymbol == null)
            //             {
            //                 // If the service is not found, no need to generate
            //                 return;
            //             }
            //
            //             // Generate the controller code
            //             string controllerSource =
            //                 $@"
            // using Microsoft.AspNetCore.Mvc;
            // using System.Collections.Generic;
            // using System.Threading.Tasks;
            // using ApiGenerator.Source.Services;
            //
            // namespace GeneratedControllers
            // {{
            //     [ApiController]
            //     [Route(""api/[controller]"")]
            //     public class WeatherController : ControllerBase
            //     {{
            //         private readonly WeatherService _service;
            //
            //         public WeatherController(WeatherService service)
            //         {{
            //             _service = service;
            //         }}
            //
            //         [HttpGet]
            //         public async Task<IActionResult> Get() => Ok(await _service.GetWeatherAsync());
            //
            //     }}
            // }}
            // ";
            //
            //             context.AddSource(
            //                 "WeatherController.g.cs",
            //                 SourceText.From(controllerSource, Encoding.UTF8)
            //             );


            IEnumerable<INamedTypeSymbol> serviceClasses = context
                .Compilation.GlobalNamespace.GetAllTypes()
                .Where(type =>
                    type.Name.EndsWith("Service")
                    && type.TypeKind == TypeKind.Class
                    && type.DeclaredAccessibility == Accessibility.Public
                );

            foreach (INamedTypeSymbol serviceClass in serviceClasses)
            {
                GenerateControllerForService(serviceClass, context);
            }
        }

        private void GenerateControllerForService(
            INamedTypeSymbol @class,
            GeneratorExecutionContext context
        )
        {
            string className = @class.Name;
            string controllerName = className.Replace("Service", "Controller");
            const string controllerNamespace = "Generated.Controllers";
            string serviceNamespace = @class.ContainingNamespace.ToDisplayString();

            List<IMethodSymbol> methods = @class
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.DeclaredAccessibility == Accessibility.Public)
                .ToList();

            StringBuilder sourceBuilder = new StringBuilder(
                $@"
            using Microsoft.AspNetCore.Mvc;
            using System.Collections.Generic;
            using System.Threading.Tasks;
            using {serviceNamespace};
            namespace {controllerNamespace}
            {{
                [ApiController]
                [Route(""api/[controller]"")]
                public class {controllerName} : ControllerBase
                {{
                    private readonly {className} _service;
                    public {controllerName}({className} service)
                    {{
                        _service = service;
                    }}
            "
            );

            foreach (IMethodSymbol method in methods)
            {
                string methodsName = method.Name;
                string returnType = method.ReturnType.ToDisplayString();

                string[] parameters = method
                    .Parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}")
                    .ToArray();

                string requestParams = parameters.Length == 1 ? parameters[0] : "object";
                string route = $"[HttpPost(\"{methodsName}\")]";
                string paramDeclaration =
                    parameters.Length == 0 ? ""
                    : parameters.Length == 1 ? $"[FromBody] {parameters[0]}"
                    : $"params {string.Join(", ", parameters)}";

                string collParams =
                    parameters.Length == 1
                        ? parameters[0].Split(' ')[1]
                        : "// handle param binding manually";

                sourceBuilder.AppendLine(
                    $@"
                    {route}
                    public async Task<{returnType}> {methodsName}({paramDeclaration})
                    {{
            Console.WriteLine($""Found  service classes."");
                        // Call the service method
                        return await _service.{methodsName}({collParams});
                    }}
            }}"
                );

                context.AddSource(
                    $"{controllerName}.g.cs",
                    SourceText.From(sourceBuilder.ToString(), Encoding.UTF8)
                );
            }
        }

        //             string className = serviceClass.Name;
        //             string controllerName = className.Replace("Service", "Controller");
        //             // Determine the namespace for the generated controller.
        //             // Using serviceClass.ContainingNamespace ensures the generated controller is in a logical place.
        //             string controllerNamespace = "Generated.Controllers";
        //             string serviceNamespace = serviceClass.ContainingNamespace.ToDisplayString();
        //
        //             List<IMethodSymbol> publicMethods = serviceClass
        //                 .GetMembers()
        //                 .OfType<IMethodSymbol>()
        //                 .Where(m =>
        //                     m.DeclaredAccessibility == Accessibility.Public
        //                     && !m.IsStatic
        //                     && !m.IsGenericMethod
        //                 ) // Exclude static and generic methods
        //                 .ToList();
        //
        //             if (!publicMethods.Any())
        //             {
        //                 return; // No public methods to expose
        //             }
        //
        //             StringBuilder sourceBuilder = new StringBuilder();
        //
        //             sourceBuilder.AppendLine(
        //                 $@"// <auto-generated/>
        // using Microsoft.AspNetCore.Mvc;
        // using System.Collections.Generic;
        // using System.Threading.Tasks;
        // using {serviceNamespace}; // Ensure the service namespace is imported
        //
        // namespace {controllerNamespace}
        // {{
        //     [ApiController]
        //     [Route(""api/[controller]"")]
        //     public class {controllerName} : ControllerBase
        //     {{
        //         private readonly {className} _service;
        //
        //         public {controllerName}({className} service)
        //         {{
        //             _service = service;
        //         }}
        // "
        //             );
        //
        //             foreach (IMethodSymbol method in publicMethods)
        //             {
        //                 string methodName = method.Name;
        //                 string returnType = method.ReturnType.ToDisplayString();
        //                 string methodParameters = string.Join(
        //                     ", ",
        //                     method.Parameters.Select(p => $"{p.Type.ToDisplayString()} {p.Name}")
        //                 );
        //                 string methodArguments = string.Join(", ", method.Parameters.Select(p => p.Name));
        //
        //                 // Determine the HTTP verb and route based on method name or attributes if you want to get more sophisticated.
        //                 // For simplicity, we'll use POST for methods with parameters and GET for methods without.
        //                 string httpVerbAttribute = "[HttpPost]"; // Default to POST
        //                 string route = "";
        //
        //                 if (!method.Parameters.Any())
        //                 {
        //                     httpVerbAttribute = "[HttpGet]";
        //                     route = $@"        [Route(""{methodName}"")]"; // Append method name to route for clarity
        //                 }
        //                 else if (method.Parameters.Count() == 1)
        //                 {
        //                     // If there's one parameter, we can try to bind it from the body.
        //                     // For more complex scenarios, you'd need to inspect parameter types (e.g., [FromQuery], [FromRoute]).
        //                     methodParameters = $"[FromBody] {methodParameters}";
        //                     route = $@"        [HttpPost(""{methodName}"")]";
        //                 }
        //                 else
        //                 {
        //                     // For multiple parameters, you'd typically expect a single DTO as a [FromBody] parameter.
        //                     // This is a simplification; a real-world scenario might need more robust parameter handling.
        //                     // Or you'd enforce a single request object for POSTs.
        //                     httpVerbAttribute = "[HttpPost]";
        //                     route = $@"        [Route(""{methodName}"")]";
        //                 }
        //
        //                 // Handle async methods: Ensure the return type is wrapped in Task<T> if the method itself returns Task or Task<T>
        //                 string actualReturnType = returnType;
        //                 if (
        //                     returnType.StartsWith("System.Threading.Tasks.Task<")
        //                     && returnType.EndsWith(">")
        //                 )
        //                 {
        //                     actualReturnType = returnType.Substring(
        //                         "System.Threading.Tasks.Task<".Length,
        //                         returnType.Length - "System.Threading.Tasks.Task<".Length - 1
        //                     );
        //                 }
        //                 else if (returnType == "System.Threading.Tasks.Task")
        //                 {
        //                     actualReturnType = "IActionResult"; // Or void/Task for fire-and-forget
        //                 }
        //
        //                 string awaitKeyword = returnType.Contains("Task") ? "await " : "";
        //                 string returnStatement =
        //                     actualReturnType != "void"
        //                     && actualReturnType != "IActionResult"
        //                     && actualReturnType != "Task"
        //                         ? "return Ok("
        //                         : "return ";
        //                 string endReturnStatement =
        //                     actualReturnType != "void"
        //                     && actualReturnType != "IActionResult"
        //                     && actualReturnType != "Task"
        //                         ? ");"
        //                         : ";";
        //
        //                 // If it's a Task, ensure we await it. If it's Task<T>, unwrap. If it's void Task, return Ok().
        //                 string methodCall = $"{awaitKeyword}_service.{methodName}({methodArguments})";
        //                 string controllerActionContent;
        //
        //                 if (returnType == "System.Threading.Tasks.Task")
        //                 {
        //                     controllerActionContent =
        //                         $@"{awaitKeyword}_service.{methodName}({methodArguments});
        //             return Ok();";
        //                 }
        //                 else if (returnType.StartsWith("System.Threading.Tasks.Task<"))
        //                 {
        //                     controllerActionContent =
        //                         $"return Ok({awaitKeyword}_service.{methodName}({methodArguments}));";
        //                 }
        //                 else if (actualReturnType == "void") // Synchronous void method
        //                 {
        //                     controllerActionContent =
        //                         $@"_service.{methodName}({methodArguments});
        //             return Ok();";
        //                 }
        //                 else // Synchronous method with return value
        //                 {
        //                     controllerActionContent =
        //                         $"return Ok(_service.{methodName}({methodArguments}));";
        //                 }
        //
        //                 sourceBuilder.AppendLine(
        //                     $@"
        //         {httpVerbAttribute}{route}
        //         public async {returnType} {methodName}({methodParameters})
        //         {{
        //             {controllerActionContent}
        //         }}
        // "
        //                 );
        //             }
        //
        //             sourceBuilder.AppendLine(
        //                 @"    }
        // }"
        //             );
        //
        //             context.AddSource(
        //                 $"{controllerName}.g.cs",
        //                 SourceText.From(sourceBuilder.ToString(), Encoding.UTF8)
        //             );
        // }

        private static IEnumerable<INamespaceSymbol> GetAllNamespaces(INamespaceSymbol ns)
        {
            yield return ns;
            foreach (INamespaceSymbol child in ns.GetNamespaceMembers())
            {
                foreach (INamespaceSymbol nested in GetAllNamespaces(child))
                {
                    yield return nested;
                }
            }
        }
    }

    public static class Extensions
    {
        public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol ns)
        {
            foreach (INamedTypeSymbol type in ns.GetTypeMembers())
            {
                yield return type;
            }

            foreach (INamespaceSymbol childNs in ns.GetNamespaceMembers())
            {
                foreach (INamedTypeSymbol type in GetAllTypes(childNs))
                {
                    yield return type;
                }
            }
        }
    }
}
