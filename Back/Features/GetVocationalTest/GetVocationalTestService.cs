namespace Vocanator.Back.Features.GetVocationalTest;

public class GetVocationalTestService(VocanatorDbContext ctx) : IAppService
{
    public async Task<GetVocationalTestOut> Get(Guid id)
    {
        var vocationalTest = await ctx.VocationalTests.AsNoTracking()
            .Where(x => x.Id == id)
            .Include(x => x.Results)
            .FirstOrDefaultAsync();

        if (vocationalTest == null) return new();

        var user = await ctx.Users.AsNoTracking()
            .Where(x => x.Id == vocationalTest.UserId)
            .FirstAsync();

        var areas = await ctx.KnowledgeAreas.AsNoTracking().ToListAsync();

        var hanking = vocationalTest.Results
            .OrderByDescending(r => r.Score)
            .Take(3)
            .Join(areas, r => r.KnowledgeAreaId, a => a.Id,
                (r, a) => new GetVocationalTestItemOut { Area = a.Name, Score = r.Score })
            .ToList();

        return new GetVocationalTestOut
        {
            Name = user.Name,
            Email = user.Email,
            Hanking = hanking,
        };
    }
}
