using Vocanator.Back.Domain;

namespace Vocanator.Back.Features.GetQuestions;

public class GetQuestionsService(VocanatorDbContext ctx) : IAppService
{
    public async Task<List<Question>> Get()
    {
        var questions = await ctx.Questions.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync();

        return questions;
    }
}
