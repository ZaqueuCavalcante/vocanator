namespace Vocanator.Back.Features.CreateVocationalTest;

[ApiController]
public class CreateVocationalTestController(CreateVocationalTestService service) : ControllerBase
{
    /// <summary>
    /// Quiz
    /// </summary>
    /// <remarks>
    /// Cria um novo quiz.
    /// </remarks>
    [HttpPost("vocational-tests")]
    public async Task<IActionResult> Create([FromBody] CreateVocationalTestIn data)
    {
        var result = await service.Create(data);
        return Ok(result);
    }
}
