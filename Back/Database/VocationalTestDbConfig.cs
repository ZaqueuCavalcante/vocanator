using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public class VocationalTestDbConfig : IEntityTypeConfiguration<VocationalTest>
{
    public void Configure(EntityTypeBuilder<VocationalTest> entity)
    {
        entity.ToTable("vocational_tests");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(x => x.CreatedAt).HasColumnType("timestamp");
    }
}
