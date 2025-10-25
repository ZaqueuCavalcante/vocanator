namespace Vocanator.Back.Domain;

public class VocationalTestResult
{
    public Guid VocationalTestId { get; set; }
    public int KnowledgeAreaId { get; set; }
    public int Score { get; set; }

    public VocationalTestResult() { }

    public VocationalTestResult(Guid vocationalTestId, int knowledgeAreaId, int score)
    {
        VocationalTestId = vocationalTestId;
        KnowledgeAreaId = knowledgeAreaId;
        Score = score;
    }
}
