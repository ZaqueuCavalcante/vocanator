namespace Vocanator.Back.Configs;

public static class SettingsConfigs
{
    public static void AddSettingsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<DatabaseSettings>();
    }
}
