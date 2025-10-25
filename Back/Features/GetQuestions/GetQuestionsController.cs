namespace Vocanator.Back.Features.GetQuestions;

[ApiController]
public class GetQuestionsController(GetQuestionsService service) : ControllerBase
{
    /// <summary>
    /// Perguntas
    /// </summary>
    /// <remarks>
    /// Retorna as perguntas.
    /// </remarks>
    [HttpGet("questions")]
    public async Task<IActionResult> Get()
    {
        var questions = await service.Get();
        return Ok(questions);
    }
}
