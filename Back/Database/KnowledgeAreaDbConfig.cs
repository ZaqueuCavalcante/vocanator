using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public class KnowledgeAreaDbConfig : IEntityTypeConfiguration<KnowledgeArea>
{
    public void Configure(EntityTypeBuilder<KnowledgeArea> entity)
    {
        entity.ToTable("knowledge_areas");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();
    }
}
