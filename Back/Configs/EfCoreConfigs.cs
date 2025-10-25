using Npgsql;
using Vocanator.Back.Database;

namespace Vocanator.Back.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(sp =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.Database().ConnectionString);

            return dataSourceBuilder.Build();
        });

        builder.Services.AddDbContext<VocanatorDbContext>();
    }
}
