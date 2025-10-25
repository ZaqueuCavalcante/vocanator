namespace Vocanator.Back.Features.CreateVocationalTest;

public class CreateVocationalTestIn
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<CreateVocationalTestQuestionIn> Responses { get; set; } = [];
}

public class CreateVocationalTestQuestionIn
{
    public int QuestionId { get; set; }
    public bool Yes { get; set; }
}
