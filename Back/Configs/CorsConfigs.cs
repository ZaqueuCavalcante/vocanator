namespace Vocanator.Back.Configs;

public static class CorsConfigs
{
    public static void AddCorsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options
            .AddDefaultPolicy(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(
                    "http://localhost:5173",
                    "https://vocanator.up.railway.app"
                )
            )
        );
    }
}
