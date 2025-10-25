namespace Vocanator.Back.Domain;

public class KnowledgeArea
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public KnowledgeArea() { }

    public KnowledgeArea(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
