using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public class VocationalTestResultDbConfig : IEntityTypeConfiguration<VocationalTestResult>
{
    public void Configure(EntityTypeBuilder<VocationalTestResult> entity)
    {
        entity.ToTable("vocational_test_results");

        entity.HasKey(e => new { e.VocationalTestId, e.KnowledgeAreaId });

        entity.HasOne<VocationalTest>()
            .WithMany(t => t.Results)
            .HasPrincipalKey(k => k.Id)
            .HasForeignKey(e => e.VocationalTestId);

        entity.HasOne<KnowledgeArea>()
            .WithMany()
            .HasPrincipalKey(k => k.Id)
            .HasForeignKey(e => e.KnowledgeAreaId);
    }
}
