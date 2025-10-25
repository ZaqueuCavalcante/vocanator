namespace Vocanator.Back.Features.GetVocationalTest;

public class GetVocationalTestOut
{
    public List<GetVocationalTestItemOut> Hanking { get; set; } = [];
}

public class GetVocationalTestItemOut
{
    public string Area { get; set; }
    public int Score { get; set; }
}
