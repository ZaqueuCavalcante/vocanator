using Vocanator.Back.Domain;

namespace Vocanator.Back.Features.CreateVocationalTest;

public class CreateVocationalTestService(VocanatorDbContext ctx) : IAppService
{
    public async Task<VocationalTest> Create(CreateVocationalTestIn data)
    {
        var email = data.Email.ToLowerInvariant();
        var userExists = await ctx.Users.AnyAsync(x => x.Email == email);

        var user = userExists ? await ctx.Users.FirstAsync(x => x.Email == email) : new AppUser(data.Name, email);
        if (!userExists) ctx.Add(user);

        var questions = await ctx.Questions.AsNoTracking().OrderBy(x => x.Id).ToListAsync();
        var weights = await ctx.Weights.AsNoTracking().OrderBy(x => x.QuestionId).ToListAsync();
        var areas = await ctx.KnowledgeAreas.AsNoTracking().OrderBy(x => x.Id).ToListAsync();

        var results = areas.ToDictionary(a => a.Id, _ => 0);

        foreach (var response in data.Responses)
        {
            var (questionId, sim) = (response.QuestionId, response.Yes);
            var relatedWeights = weights.Where(w => w.QuestionId == questionId);

            foreach (var w in relatedWeights)
            {
                results[w.KnowledgeAreaId] += sim ? w.Weight : (6 - w.Weight);
            }
        }

        var vocationalTest = new VocationalTest(user.Id);

        var finalResults = results.Select(r => new VocationalTestResult(
            vocationalTestId: vocationalTest.Id,
            knowledgeAreaId: r.Key,
            score: r.Value
        )).ToList();

        ctx.Add(vocationalTest);
        ctx.AddRange(finalResults);
        await ctx.SaveChangesAsync();

        return vocationalTest;
    }
}
