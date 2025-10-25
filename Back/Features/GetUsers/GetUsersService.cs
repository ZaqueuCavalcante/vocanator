using Vocanator.Back.Domain;

namespace Vocanator.Back.Features.GetUsers;

public class GetUsersService(VocanatorDbContext ctx) : IAppService
{
    public async Task<List<AppUser>> Get()
    {
        var users = await ctx.Users.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return users;
    }
}
