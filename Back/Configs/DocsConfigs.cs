using System.Reflection;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

namespace Vocanator.Back.Configs;

public static class DocsConfigs
{
    public static void AddDocsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Vocanator API Docs",
                Description = ReadResource("api-intro.md"),
            });

            options.EnableAnnotations();

            options.DocInclusionPredicate((name, api) => true);

            options.ExampleFilters();

            options.DescribeAllParametersInCamelCase();

            var xmlPath = Path.Combine(AppContext.BaseDirectory, "Back.xml");
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        });

        builder.Services.AddSwaggerExamplesFromAssemblyOf(typeof(Program));
        builder.Services.AddOpenApi();
    }

    public static void MapScalarDocs(this IEndpointRouteBuilder options)
    {
        options.MapScalarApiReference("/docs", options =>
        {
            options.WithModels(false);
            options.WithTitle("Vocanator API Docs");
            options.WithDocumentDownloadType(DocumentDownloadType.Json);
            options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");

            options.CustomCss = @"
                :root {
                    --scalar-sidebar-width: 300px;
                }
            ";
        });
    }

    public static void UseDocs(this IApplicationBuilder app)
    {
        app.UseStaticFiles();
        app.UseSwagger();
    }

    private static string ReadResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
