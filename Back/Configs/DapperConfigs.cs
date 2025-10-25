using Dapper;

namespace Vocanator.Back.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this WebApplicationBuilder _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}
