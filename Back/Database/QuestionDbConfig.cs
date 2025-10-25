using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public class QuestionDbConfig : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> entity)
    {
        entity.ToTable("questions");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();
    }
}
