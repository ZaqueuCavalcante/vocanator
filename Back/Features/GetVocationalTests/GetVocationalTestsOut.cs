using Vocanator.Back.Features.GetVocationalTest;

namespace Vocanator.Back.Features.GetVocationalTests;

public class GetVocationalTestsOut
{
    public int Total { get; set; }

    public List<GetVocationalTestOut> Tests { get; set; } = [];
}
