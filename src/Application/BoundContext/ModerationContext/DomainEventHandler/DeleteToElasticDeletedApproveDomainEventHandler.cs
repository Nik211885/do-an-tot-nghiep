using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Models;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class DeleteToElasticDeletedApproveDomainEventHandler(
    IElasticFactory elasticFactory,
    ILogger<DeleteToElasticDeletedApproveDomainEventHandler> logger)
    : IEventHandler<DeletedApproveDomainEvent>
{
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    private readonly ILogger<DeleteToElasticDeletedApproveDomainEventHandler> _logger = logger;
    public async Task Handler(DeletedApproveDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var elasticChapter = _elasticFactory.GetInstance<ChapterEmbeddingModel>();
        await elasticChapter.DeleteByQueryAsync(q => q
            .Term(t => t.Field(f => f.ChapterId)
                .Value(domainEvent.BookApproval.ChapterId.ToString())));
    }
}
