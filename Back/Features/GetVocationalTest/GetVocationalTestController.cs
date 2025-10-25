namespace Vocanator.Back.Features.GetVocationalTest;

[ApiController]
public class GetVocationalTestController(GetVocationalTestService service) : ControllerBase
{
    /// <summary>
    /// Quiz
    /// </summary>
    /// <remarks>
    /// Retorna o quiz especificado.
    /// </remarks>
    [HttpGet("vocational-tests/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var vocationalTest = await service.Get(id);
        return Ok(vocationalTest);
    }
}
