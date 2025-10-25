using Npgsql;
using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public class VocanatorDbContext(DbContextOptions<VocanatorDbContext> options, NpgsqlDataSource npgsqlDataSource) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(npgsqlDataSource);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("vocanator");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }
}
