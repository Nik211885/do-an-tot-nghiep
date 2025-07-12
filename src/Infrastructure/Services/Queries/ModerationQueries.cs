using Application.BoundContext.ModerationContext.Queries;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Queries;

public class ModerationQueries(ModerationDbContext moderationDbContext) : IModerationQueries
{
    private readonly ModerationDbContext _moderationDbContext = moderationDbContext;
}
