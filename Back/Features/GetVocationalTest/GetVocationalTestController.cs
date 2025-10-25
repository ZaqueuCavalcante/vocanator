namespace Vocanator.Back.Features.GetVocationalTest;

[ApiController]
public class GetVocationalTestController(GetVocationalTestService service) : ControllerBase
{
    /// <summary>
    /// Teste vocacional
    /// </summary>
    /// <remarks>
    /// Retorna o teste vocacional especificado.
    /// </remarks>
    [HttpGet("vocational-tests/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var vocationalTest = await service.Get(id);
        return Ok(vocationalTest);
    }
}
