namespace Vocanator.Back.Domain;

public class VocationalTest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public VocationalTest() { }

    public VocationalTest(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }
}
