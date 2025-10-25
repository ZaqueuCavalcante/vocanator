using Vocanator.Back.Middlewares;

namespace Vocanator.Back.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
            options.Filters.Add(new ConsumesAttribute("application/json"));
        });

        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddHttpClient();
    }

    public static void UseExceptions(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
    }

    public static void UseControllers(this IApplicationBuilder app)
    {
        app.UseEndpoints(options =>
        {
            options.MapControllers();

            options.MapOpenApi();
            options.MapScalarDocs();
        });
    }
}
