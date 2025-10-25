namespace Vocanator.Back.Features.GetVocationalTests;

[ApiController]
public class GetVocationalTestsController(GetVocationalTestsService service) : ControllerBase
{
    /// <summary>
    /// Quizzes
    /// </summary>
    /// <remarks>
    /// Retorna os quizzes.
    /// </remarks>
    [HttpGet("vocational-tests")]
    public async Task<IActionResult> Get()
    {
        var vocationalTests = await service.Get();
        return Ok(vocationalTests);
    }
}
