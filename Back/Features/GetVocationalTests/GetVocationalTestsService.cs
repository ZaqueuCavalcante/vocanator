using Vocanator.Back.Features.GetVocationalTest;

namespace Vocanator.Back.Features.GetVocationalTests;

public class GetVocationalTestsService(VocanatorDbContext ctx) : IAppService
{
    public async Task<GetVocationalTestsOut> Get()
    {
        var vocationalTests = await ctx.VocationalTests.AsNoTracking()
            .Include(x => x.Results)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var ids = vocationalTests.Select(vt => vt.UserId).Distinct().ToList();

        var users = await ctx.Users.AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        var areas = await ctx.KnowledgeAreas.AsNoTracking().ToListAsync();

        var result = new List<GetVocationalTestOut>();

        foreach (var vocationalTest in vocationalTests)
        {
            var user = users[vocationalTest.UserId];

            var hanking = vocationalTest.Results
                .OrderByDescending(r => r.Score)
                .Take(3)
                .Join(areas, r => r.KnowledgeAreaId, a => a.Id,
                    (r, a) => new GetVocationalTestItemOut { Area = a.Name, Score = r.Score })
                .ToList();

            result.Add(new GetVocationalTestOut { Name = user.Name, Email = user.Email, Hanking = hanking });
        }

        return new GetVocationalTestsOut { Total = result.Count, Tests = result };
    }
}
