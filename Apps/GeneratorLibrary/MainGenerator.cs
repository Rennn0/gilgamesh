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
            Console.WriteLine($"Found  service classes.");

            IEnumerable<INamedTypeSymbol> serviceClasses =
                context
                    .Compilation.GlobalNamespace.GetNamespaceMembers()
                    .SelectMany(ns => ns.GetNamespaceMembers())
                    .FirstOrDefault(ns => ns.Name == "Services")
                    ?.GetTypeMembers()
                    .Where(t =>
                        t.DeclaredAccessibility == Accessibility.Public
                        && t.TypeKind == TypeKind.Class
                    ) ?? Enumerable.Empty<INamedTypeSymbol>();
            Console.WriteLine($"Found {serviceClasses.Count()} service classes.");
            foreach (INamedTypeSymbol @class in serviceClasses)
            {
                Console.WriteLine($"GenerateControllerForService");
                GenerateControllerForService(@class, context);
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
    }
}
