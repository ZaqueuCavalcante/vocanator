namespace Vocanator.Back.Features.CreateVocationalTest;

[ApiController]
public class CreateVocationalTestController(CreateVocationalTestService service) : ControllerBase
{
    /// <summary>
    /// Teste vocacional
    /// </summary>
    /// <remarks>
    /// Cria um novo teste vocacional.
    /// </remarks>
    [HttpPost("vocational-tests")]
    public async Task<IActionResult> Create([FromBody] CreateVocationalTestIn data)
    {
        var result = await service.Create(data);
        return Ok(result);
    }
}
