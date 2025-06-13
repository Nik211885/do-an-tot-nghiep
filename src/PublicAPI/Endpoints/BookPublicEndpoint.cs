using Application.Interfaces.Cache;
using Application.Interfaces.Elastic;
using Application.Models;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;
using QueryParamRequest = Application.Models.QueryParamRequest;

namespace PublicAPI.Endpoints;

public class BookPublicEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("public-books");
        apis.MapGet("genre", BookPublicEndpointServices.GetWithPaginationBookByGenre)
            .WithTags("PublicBooks")
            .WithName("GetPublicBookByGenre")
            .WithDescription("Get public book by genre.");
        apis.MapGet("policy", BookPublicEndpointServices.GetWithPaginationBookByPolicy)
            .WithTags("PublicBooks")
            .WithDescription("Get public book by policy.");
        apis.MapGet("all", BookPublicEndpointServices.GetWithPaginationBook)
            .WithTags("PublicBooks")
            .WithName("GetAllPublicBooks")
            .WithDescription("Get All public books.");
    }
}

public static class BookPublicEndpointServices
{
    private static QueryParamRequest _getSimpleQueryParam(PaginationRequest page)
        => new QueryParamRequest()
        {
            Page = page.PageNumber,
            PageSize = page.PageSize,
            Sort = $"{nameof(BookElasticModel.CreateDateTimeOffset)}:desc",
        };

    private static Action<QueryDescriptor<BookElasticModel>> BookActive
        => q => q.Term(t => t.Field(f => f.IsActive).Value(false));
        
    public static async Task<Results<Ok<PaginationItem<BookElasticModel>>,NotFound, ProblemHttpResult>>
        GetWithPaginationBookByGenre(
            [FromQuery] string slug,
            [AsParameters] PaginationRequest page,
            [FromServices] BookPublicEndpointServiceWrapper service
            )
    {
        var result = await service
            .BookElasticServices
            .PaginatedListAsync(_getSimpleQueryParam(page), q=>q
                .Bool(b=>b.Must(
                        m => m.Term(t => t.Field("genres.slug.keyword"!).Value(slug)),
                        BookActive
                    )
                )
            );
        

        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<PaginationItem<BookElasticModel>>, NotFound, ProblemHttpResult>>
        GetWithPaginationBookByPolicy(
            [FromQuery] BookPolicy policy,
            [AsParameters] PaginationRequest page,
            [FromServices] BookPublicEndpointServiceWrapper service
            )
    {
        service.Logger.LogInformation("find book with book policy is {@Policy}",policy.ToString());
        var result = await service
            .BookElasticServices.PaginatedListAsync(_getSimpleQueryParam(page), q=>q
                .Bool(m=>m.Must(
                    b=>b.Term(t=>
                        t.Field("bookPolicy.keyword"!).Value(policy.ToString())),
                    BookActive
                    )
                )
            );
        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<PaginationItem<BookElasticModel>>, NotFound, ProblemHttpResult>>
        GetWithPaginationBook(
            [AsParameters] PaginationRequest page,
            [FromServices] BookPublicEndpointServiceWrapper service)
    {
        var result = await service.BookElasticServices
            .PaginatedListAsync(_getSimpleQueryParam(page), q=>q.Bool(
                b=>b.Must(BookActive)));
        return TypedResults.Ok(result);
    }
}

public class BookPublicEndpointServiceWrapper(IElasticFactory elasticFactory,
    ILogger<BookPublicEndpointServiceWrapper> logger,
    ICache cache)
{
    public IElasticServices<BookElasticModel> BookElasticServices { get; } =
        elasticFactory.GetInstance<BookElasticModel>();
    public ICache Cache { get; } = cache;
    public ILogger<BookPublicEndpointServiceWrapper> Logger { get; } = logger;
}
