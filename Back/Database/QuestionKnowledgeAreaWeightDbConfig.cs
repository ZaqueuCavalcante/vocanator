using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public class QuestionKnowledgeAreaWeightDbConfig : IEntityTypeConfiguration<QuestionKnowledgeAreaWeight>
{
    public void Configure(EntityTypeBuilder<QuestionKnowledgeAreaWeight> entity)
    {
        entity.ToTable("question_knowledge_areas_weights");

        entity.HasKey(e => new { e.QuestionId, e.KnowledgeAreaId });

        entity.HasOne<Question>()
            .WithMany()
            .HasPrincipalKey(q => q.Id)
            .HasForeignKey(e => e.QuestionId);

        entity.HasOne<KnowledgeArea>()
            .WithMany()
            .HasPrincipalKey(k => k.Id)
            .HasForeignKey(e => e.KnowledgeAreaId);
    }
}
