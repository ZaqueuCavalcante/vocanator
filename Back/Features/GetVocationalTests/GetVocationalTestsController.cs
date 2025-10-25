namespace Vocanator.Back.Features.GetVocationalTests;

[ApiController]
public class GetVocationalTestsController(GetVocationalTestsService service) : ControllerBase
{
    /// <summary>
    /// Testes vocacionais
    /// </summary>
    /// <remarks>
    /// Retorna os testes vocacionais.
    /// </remarks>
    [HttpGet("vocational-tests")]
    public async Task<IActionResult> Get()
    {
        var vocationalTests = await service.Get();
        return Ok(vocationalTests);
    }
}
