using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.ProcessData;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class BookApprovedIndexVectorEmbeddingDomainEventHandler(
    ILogger<BookApprovedIndexVectorEmbeddingDomainEventHandler> logger,
    ICache cache,
    IVectorEmbeddingTextService vectorEmbedding,
    IPlagiarismCheckerService plagiarismCheckerService)
    : IEventHandler<ApprovedBookDomainEvent>
{
    private readonly ICache _cache = cache;
    private readonly ILogger<BookApprovedIndexVectorEmbeddingDomainEventHandler> _logger = logger;
    private readonly IPlagiarismCheckerService _plagiarismCheckerService = plagiarismCheckerService;
    private readonly IVectorEmbeddingTextService _vectorEmbedding = vectorEmbedding;
    public async Task Handler(ApprovedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var approval = domainEvent.Approval;
        _logger.LogInformation("Start write index vector embedding for {a@pproval}", approval);
        var key = string.Format(CacheKey.ModerationVectorEmbedding, approval.Id);
        List<DocSource>? docSources = await _cache.GetAsync<List<DocSource>>(key) 
                                      ?? await _vectorEmbedding.GetVectorEmbeddingFromTextAsync(approval.ContentHash);
        ArgumentNullException.ThrowIfNull(docSources);
        var docChunks = docSources.Select((x, index)=>new DocumentChunk(
                docId: approval.ChapterId.ToString(),
                docCout: approval.ContentHash.Length,
                chunkCount: x.WordCount,
                content: x.Content,
                chunkIndex: index,
                embedding: x.Embedding.ToArray()
            ));
        await _plagiarismCheckerService.AddBulkDocumentChunkAsync(docChunks);
    }
}
