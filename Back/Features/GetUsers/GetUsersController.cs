namespace Vocanator.Back.Features.GetUsers;

[ApiController]
public class GetUsersController(GetUsersService service) : ControllerBase
{
    /// <summary>
    /// Usuários
    /// </summary>
    /// <remarks>
    /// Retorna os usuários.
    /// </remarks>
    [HttpGet("users")]
    public async Task<IActionResult> Get()
    {
        var users = await service.Get();
        return Ok(users);
    }
}
