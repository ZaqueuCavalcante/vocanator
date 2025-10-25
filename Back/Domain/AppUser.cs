namespace Vocanator.Back.Domain;

public class AppUser
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }

    public AppUser() { }

    public AppUser(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        CreatedAt = DateTime.Now;
    }
}
