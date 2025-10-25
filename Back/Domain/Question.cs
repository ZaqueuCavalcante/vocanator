namespace Vocanator.Back.Domain;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }

    public Question() { }

    public Question(int id, string text)
    {
        Id = id;
        Text = text;
    }
}
