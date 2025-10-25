using Vocanator.Back.Domain;

namespace Vocanator.Back.Features.GetKnowledgeAreas;

public class GetKnowledgeAreasService(VocanatorDbContext ctx) : IAppService
{
    public async Task<List<KnowledgeArea>> Get()
    {
        var areas = await ctx.KnowledgeAreas.AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return areas;
    }
}
