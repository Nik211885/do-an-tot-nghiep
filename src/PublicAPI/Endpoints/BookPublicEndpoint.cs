    using Application.Helper;
    using Application.Interfaces.Cache;
    using Application.Interfaces.Elastic;
    using Application.Interfaces.EventBus;
    using Application.Interfaces.IdentityProvider;
    using Application.Models;
    using Application.Models.EventBus;
    using Core.BoundContext.BookAuthoringContext.BookAggregate;
    using Elastic.Clients.Elasticsearch;
    using Elastic.Clients.Elasticsearch.QueryDsl;
    using Infrastructure.Helper;
    using MassTransit;
    using Microsoft.AspNetCore.Authorization;
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
            apis.MapGet("slug", BookPublicEndpointServices.GetBookBySlug)
                .WithTags("PublicBooks")
                .WithName("PublicBooksGetBookBySlug")
                .WithDescription("Public books get public book by Slug.");
            apis.MapGet("policy", BookPublicEndpointServices.GetWithPaginationBookByPolicy)
                .WithTags("PublicBooks")
                .WithDescription("Get public book by policy.");
            apis.MapGet("all", BookPublicEndpointServices.GetWithPaginationBook)
                .WithTags("PublicBooks")
                .WithName("GetAllPublicBooks")
                .WithDescription("Get All public books.");
            apis.MapGet("author", BookPublicEndpointServices.GetWithPaginationByAuthor)
                .WithTags("PublicBooks")
                .WithName("GetPublicBooksByAuthor")
                .WithDescription("Get  public books by author.");
            apis.MapGet("search", BookPublicEndpointServices.SearchWithPaginationWithWordSentence)
                .WithTags("PublicBooks")
                .WithName("SearchPublicBooksByWordSentence")
                .WithDescription("Search public books by word sentence.");
            apis.MapGet("my-book", BookPublicEndpointServices.GetMyBookWithPagination)
                .WithTags("PublicBooks")
                .WithName("GetMyBookWithPagination")
                .WithDescription("Get my public books by.");
            apis.MapPost("by-ids", BookPublicEndpointServices.GetBookInIds)
                .WithTags("PublicBooks")
                .WithName("GetBookByIds")
                .WithDescription("Get book by ids collection.");
            apis.MapGet("tag", BookPublicEndpointServices.GetBookWithPaginationByTag)
                .WithTags("PublicBooks")
                .WithName("GetBookByTag")
                .WithDescription("Get book by tag with pagination.");
            apis.MapGet("bool-prefix",BookPublicEndpointServices.SearchAsYouType)
                .WithTags("PublicBooks")
                .WithName("SearchAsYouTypeTitleForBook")
                .WithDescription("Search public books by title for you type.");
        }
    }

public static class BookPublicEndpointServices
{
    public static QueryParamRequest _getSimpleQueryParam(PaginationRequest page, bool sortCreated = true)
        => new QueryParamRequest()
        {
            Page = page.PageNumber,
            PageSize = page.PageSize,
            Sort = sortCreated ? $"{nameof(BookElasticModel.CreateDateTimeOffset)}:desc" : string.Empty,
        };

    public static Action<QueryDescriptor<BookElasticModel>> BookActive
        => q => q.Bool(b=>b.Must(m=>m.Term(t=>t.Field("isActive").Value(true))));
        
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

    public static async Task<Results<Ok<PaginationItem<BookElasticModel>>, NotFound, ProblemHttpResult>>
        GetWithPaginationByAuthor(
            [FromQuery] Guid id,
            [AsParameters] PaginationRequest page,
            [FromServices] BookPublicEndpointServiceWrapper service
            )
    {
        var result = await service.BookElasticServices
            .PaginatedListAsync(_getSimpleQueryParam(page), q => q.Bool(
                    b => b.Must(BookActive,
                        m => m.Term(t => 
                            t.Field("authorId.keyword"!)
                                .Value(id.ToString())
                        )
                    )
                )
            );
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<BookElasticModel>>, NotFound, ProblemHttpResult>>
        GetMyBookWithPagination(
            [AsParameters] PaginationRequest page,
            [FromServices] BookPublicEndpointServiceWrapper service
        )
    {
        var result = await service.BookElasticServices
            .PaginatedListAsync(_getSimpleQueryParam(page), q => q
                .Bool(b => b.Must(
                    BookActive,
                    m => m.Term(t => t
                        .Field("authorId.keyword"!)
                        .Value(service.IdentityProvider.UserIdentity().ToString())))
                )
            );
        return TypedResults.Ok(result);
    }
    
    public static async Task<Results<Ok<PaginationItem<BookElasticModel>>, NotFound, ProblemHttpResult>>
        SearchWithPaginationWithWordSentence(
            [FromQuery] string search,
            [AsParameters] PaginationRequest page,
            [FromServices] BookPublicEndpointServiceWrapper service)
    {
        string cleanSearch = search.NormalizeSearchString();
         var result = await service.BookElasticServices
        .PaginatedListAsync(_getSimpleQueryParam(page, sortCreated: false), q => q
            .Bool(b => b
                .Must(BookActive)
                .Must(m => m
                    .Bool(innerBool => innerBool
                        .Should(
                            s => s.MultiMatch(mm => mm
                                .Fields(
                                    Fields.FromFields([
                                        new Field("title", 3.0f),
                                        new Field("title.keyword", 2.0f)
                                    ])
                                )
                                .Query(cleanSearch)
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(new Fuzziness("Auto"))
                                .PrefixLength(0)
                                .MaxExpansions(50)
                            ),
                            
                            s => s.MultiMatch(mm => mm
                                .Fields(
                                    Fields.FromFields([
                                        new Field("description", 1.5f)
                                    ])
                                )
                                .Query(cleanSearch)
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(new Fuzziness("Auto"))
                                .PrefixLength(1)
                                .MaxExpansions(50)
                            ),
                            
                            
                            s => s.MultiMatch(mm => mm
                                .Fields(Fields.FromFields([
                                    new Field("authorName", 2.0f),
                                    new Field("authorName.keyword", 1.8f)
                                ]))
                                .Query(cleanSearch)
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(new Fuzziness("Auto"))
                                .PrefixLength(1)
                                .MaxExpansions(50)
                            ),
                            
                            s => s.MultiMatch(mm => mm
                                .Fields(Fields.FromFields([
                                    new Field("genres.name",1.0f),
                                    new Field("genres.name.keyword", 0.8f)
                                ]))
                                .Query(cleanSearch)
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(new Fuzziness("Auto"))
                                .PrefixLength(1)
                                .MaxExpansions(50)
                            ),

                            
                            s => s.MultiMatch(mm => mm
                                .Fields(Fields.FromFields([
                                    new Field("tags",1.0f),
                                    new Field("tags.keyword", 0.8f)
                                ]))
                                .Query(cleanSearch)
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(new Fuzziness("Auto"))
                                .PrefixLength(1)
                                .MaxExpansions(50)
                            ),
                            
                            s => s.Wildcard(w => w
                                .Field(field => field.Title.Suffix("keyword"))
                                .Value($"*{cleanSearch.ToLowerInvariant()}*")
                                .Boost(0.5f)
                            ),
                            
                            s => s.Wildcard(w => w
                                .Field(field => field.AuthorName.Suffix("keyword"))
                                .Value($"*{cleanSearch.ToLowerInvariant()}*")
                                .Boost(0.4f)
                            ),
                            
                            // Prefix search cho auto-complete
                            s => s.Prefix(p => p
                                .Field(field => field.Title.Suffix("keyword"))
                                .Value(cleanSearch.ToLowerInvariant())
                                .Boost(0.6f)
                            ),
                            
                            s => s.Prefix(p => p
                                .Field(field => field.AuthorName.Suffix("keyword"))
                                .Value(cleanSearch.ToLowerInvariant())
                                .Boost(0.5f)
                            )
                        )
                        .MinimumShouldMatch(1)
                    )
                )
            )
        );
         if (service.IdentityProvider.IsAuthenticated())
         {
             var bookSearchIntegrationEvent = new BookSearchedIntegrationEvent(service.IdentityProvider.UserIdentity(),
                 search, service.Accessor.HttpContext?.GetIpAddress() ?? string.Empty, result.TotalCount
             );
             await service.EventBus.Publish(bookSearchIntegrationEvent);
         }
        return TypedResults.Ok(result);
    }
    public static async Task<Results<Ok<IReadOnlyCollection<BookElasticModel>>, ProblemHttpResult>>
        GetBookInIds(
            [FromBody] Guid[] bookId,
        [FromServices] BookPublicEndpointServiceWrapper service)
    {
        var result = await service.BookElasticServices
            .ListAsync(new QueryParamRequest(), q => q
                .Bool(b => b
                    .Must(BookActive,
                        m => m.Ids(i => i.Values(new Ids(bookId.Select(x => x.ToString()))))))
            );
        return TypedResults.Ok(result.Documents);
    }

    public static async Task<Results<Ok<IEnumerable<string>>, ProblemHttpResult>>
        SearchAsYouType(
            [FromQuery] string search,
            [FromServices] BookPublicEndpointServiceWrapper service
        )
    {
        var result = await service.BookElasticServices
            .ListAsync(new QueryParamRequest()
            {
                Page = 1,
                PageSize = 5
            },q=>q
                .Bool(b=>b.Must(
                    BookActive,
                    m=>m.MultiMatch(mn=>mn
                        .Query(search)
                        .Type(TextQueryType.BoolPrefix)
                        .Fields(Fields.FromFields([
                            new Field("title"),
                            new Field("title._2gram"),
                            new Field("title._3gram"),
                        ]))
                    )
                    )));
        var suggestions = result.Documents
            .Select(x => x.Title)
            .Where(x=>!string.IsNullOrEmpty(x));
        return TypedResults.Ok(suggestions);
    }
    public static async Task<Results<Ok<BookElasticModel>, ProblemHttpResult, NotFound>>
        GetBookBySlug(
            [FromQuery] string slug,
            [FromServices] BookPublicEndpointServiceWrapper service)
    {
        var result = await service.BookElasticServices
            .ListAsync(new QueryParamRequest(), q => q
                .Bool(b => b
                    .Must(BookActive,
                        m=>m.Term(t=>
                            t.Field("slug.keyword").Value(slug))))
            );
        return TypedResults.Ok(result.Documents.FirstOrDefault());
    }
    public static async Task<Results<Ok<PaginationItem<BookElasticModel>>, ProblemHttpResult, NotFound>>
        GetBookWithPaginationByTag(
            [FromQuery] string tag,
            [AsParameters] PaginationRequest page,
            [FromServices] BookPublicEndpointServiceWrapper service)
    {
        var result = await service.BookElasticServices
            .PaginatedListAsync(new QueryParamRequest()
                {
                    Page = page.PageNumber,
                    PageSize = page.PageSize,
                }, q => q
                .Bool(b => b
                    .Must(BookActive,
                        m=>m.Term(t=>
                            t.Field("tags.keyword").Value(tag))))
            );
        return TypedResults.Ok(result);
    }
}

public class BookPublicEndpointServiceWrapper(IElasticFactory elasticFactory,
    ILogger<BookPublicEndpointServiceWrapper> logger,
    IIdentityProvider identityProvider,
    IHttpContextAccessor accessor,
    IEventBus<BookSearchedIntegrationEvent> eventBus,
    ICache cache)
{
    public IHttpContextAccessor Accessor { get; } = accessor ;
    public IEventBus<BookSearchedIntegrationEvent> EventBus { get; } = eventBus;
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IElasticServices<BookElasticModel> BookElasticServices { get; } =
        elasticFactory.GetInstance<BookElasticModel>();
    public ICache Cache { get; } = cache;
    public ILogger<BookPublicEndpointServiceWrapper> Logger { get; } = logger;
}
