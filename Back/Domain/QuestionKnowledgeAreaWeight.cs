namespace Vocanator.Back.Domain;

public class QuestionKnowledgeAreaWeight
{
    public int QuestionId { get; set; }
    public int KnowledgeAreaId { get; set; }
    public int Weight { get; set; }

    public QuestionKnowledgeAreaWeight() { }

    public QuestionKnowledgeAreaWeight(
        int questionId = 0,
        int knowledgeAreaId = 0,
        int weight = 0)
    {
        QuestionId = questionId;
        KnowledgeAreaId = knowledgeAreaId;
        Weight = weight;
    }
}
