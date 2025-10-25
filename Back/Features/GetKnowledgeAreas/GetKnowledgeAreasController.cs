namespace Vocanator.Back.Features.GetKnowledgeAreas;

[ApiController]
public class GetKnowledgeAreasController(GetKnowledgeAreasService service) : ControllerBase
{
    /// <summary>
    /// Áreas do conhecimento
    /// </summary>
    /// <remarks>
    /// Retorna as áreas do conhecimento.
    /// </remarks>
    [HttpGet("knowledge-areas")]
    public async Task<IActionResult> Get()
    {
        var areas = await service.Get();
        return Ok(areas);
    }
}
