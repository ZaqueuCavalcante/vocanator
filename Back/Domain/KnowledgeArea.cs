namespace Vocanator.Back.Domain;

public class KnowledgeArea
{
    public int Id { get; set; }
    public string Name { get; set; }

    public KnowledgeArea() { }

    public KnowledgeArea(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
