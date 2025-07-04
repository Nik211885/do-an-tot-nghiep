using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.ProcessData;
using Application.Models;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

// Write chapter for elastic with embedding vector
public class WriteToElasticWhenApprovedBookDomainEventHandler(
    ILogger<WriteToElasticWhenApprovedBookDomainEventHandler> logger,
    IVectorEmbeddingTextService embeddingTextService,
    IElasticFactory elasticFactory,
    ICleanTextService cleanTextService)
    : IEventHandler<ApprovedBookDomainEvent>
{
    private readonly ICleanTextService _cleanTextService = cleanTextService;
    private readonly ILogger<WriteToElasticWhenApprovedBookDomainEventHandler> _logger = logger;
    private readonly IVectorEmbeddingTextService _embeddingTextService = embeddingTextService;
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    public async Task Handler(ApprovedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var elasticChapterEmbedding = _elasticFactory
            .GetInstance<ChapterEmbeddingModel>();
        var docSource = await _embeddingTextService
            .GetVectorEmbeddingFromTextAsync(_cleanTextService.RemoveHtmlTag((domainEvent.Approval.ChapterContent)));
        if (docSource is null)
        {
            _logger.LogError("Can't not create embedding document for {@approved}",domainEvent.Approval);
            throw new Exception("Can't not create embedding document");
        }
        // problem here if delete success but insert has problem data will
        // has lose
        await elasticChapterEmbedding.DeleteByQueryAsync(d
            =>d.Term(p=>
                p.Field(f=>f.ChapterId)
                    .Value(domainEvent.Approval.ChapterId.ToString())));
        var chaptersEmbedding =
            docSource.Select((x, index) => new ChapterEmbeddingModel()
            {
                Id = domainEvent.Approval.ChapterId.ToString()+$"_{index}",
                BookId = domainEvent.Approval.BookId.ToString(),
                ChapterId = domainEvent.Approval.ChapterId.ToString(),
                ChapterTitle = domainEvent.Approval.ChapterTitle ?? String.Empty,
                AuthorId = domainEvent.Approval.AuthorId.ToString(),
                Content = x.Content,
                Embeddings = x.Embedding.ToArray(),
                EmbeddingDim = x.EmbeddingDim,
                WordCout = x.WordCount,
                CharCout = x.CharCount,
                ModelName = x.ModelName,
            });
        await elasticChapterEmbedding.AddRangeAsync(chaptersEmbedding);
        _logger.LogInformation("Write embedding for chapter has {@Id} to elastic", 
            domainEvent.Approval.ChapterId);
    }
}
