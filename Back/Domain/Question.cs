namespace Vocanator.Back.Domain;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }

    public Question() { }

    public Question(string text)
    {
        Text = text;
    }
}
