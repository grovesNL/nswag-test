using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using NJsonSchema;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag.CodeGeneration.TypeScript;
using NSwag.SwaggerGeneration;

namespace ConsoleApp3
{

    class Program
    {
        static void Main(string[] args)
        {
            var file = GenerateTypeScript(typeof(A<>).Assembly.Location, new[] { typeof(A<>) }).GetAwaiter().GetResult();
        }

        public static async Task<string> GenerateTypeScript(string assemblyLocation, IEnumerable<Type> models)
        {
            var clientSettings = new SwaggerToTypeScriptClientGeneratorSettings
            {
                Template = TypeScriptTemplate.Fetch,
                GenerateDtoTypes = true,
                GenerateClientClasses = false,
                GenerateClientInterfaces = false,
                TypeScriptGeneratorSettings =
                {
                    TypeStyle = TypeScriptTypeStyle.Interface,
                    NullHandling = NullHandling.JsonSchema,
                    DateTimeType = TypeScriptDateTimeType.MomentJS
                }
            };
            var swaggerGenerator = new AssemblyTypeToSwaggerGenerator(new AssemblyTypeToSwaggerGeneratorSettings
            {
                AssemblyPath = assemblyLocation,
                DefaultPropertyNameHandling = PropertyNameHandling.CamelCase
            });
            var document = await swaggerGenerator.GenerateAsync(models.Select(m => m.FullName).ToArray());
            return new SwaggerToTypeScriptClientGenerator(document, clientSettings, new TypeScriptTypeResolver(clientSettings.TypeScriptGeneratorSettings, document)).GenerateFile();
        }
    }
}
