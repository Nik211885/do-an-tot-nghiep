using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Infrastructure.Services.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class ModerationEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("moderation");
        apis.MapPost("create/chapter", ModerationEndpointServices.CreateModeration)
            .WithTags("ModerationChapter")
            .WithName("CreatModerationChapter")
            .WithDescription("Create moderation chapter");
    }
}

public static class ModerationEndpointServices
{
    [Authorize]
    public static async Task<Results<Ok<BookApprovalViewModel>, ProblemHttpResult>> CreateModeration(
        [FromBody] CreateBookApprovalCommand command,
        [FromServices] ModerationServiceWrapper service
        )
    {
        var result = await service.FactoryHandler.Handler<CreateBookApprovalCommand, BookApprovalViewModel>(command);
        return TypedResults.Ok(result);
    }
}


public class ModerationServiceWrapper(IFactoryHandler factoryHandler,
    ILogger<BookAuthoringServiceWrapper> logger,
    IModerationQueries moderationQueries)
{
    public IModerationQueries ModerationQueries { get; } = moderationQueries;
    public IFactoryHandler FactoryHandler { get;} = factoryHandler;
    public ILogger<BookAuthoringServiceWrapper> Logger { get;} = logger;
}
