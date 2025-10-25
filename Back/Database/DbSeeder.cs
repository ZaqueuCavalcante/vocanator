using Vocanator.Back.Domain;

namespace Vocanator.Back.Database;

public static class DbSeeder
{
    public static void Seed(VocanatorDbContext dbContext)
    {
        var areas = new List<KnowledgeArea>
        {
            new("Ciências Exatas e da Terra"),
            new("Ciências Biológicas"),
            new("Ciências da Saúde"),
            new("Ciências Humanas"),
            new("Ciências Sociais Aplicadas"),
            new("Ciências Agrárias"),
            new("Engenharias"),
            new("Linguística, Letras e Artes"),
        };

        dbContext.AddRange(areas);
        dbContext.SaveChanges();
    }
}
