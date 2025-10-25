using Vocanator.Back.Domain;

namespace Vocanator.Back.Features.CreateVocationalTest;

public class CreateVocationalTestService(VocanatorDbContext ctx) : IAppService
{
    public async Task<List<AppUser>> Create()
    {
        var users = await ctx.Users.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return users;
    }
}
