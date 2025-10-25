using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public class AppUserDbConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> entity)
    {
        entity.ToTable("users");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(x => x.CreatedAt).HasColumnType("timestamp");
    }
}
