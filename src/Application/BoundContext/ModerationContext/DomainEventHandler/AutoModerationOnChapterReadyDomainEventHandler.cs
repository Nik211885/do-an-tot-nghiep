using Application.BoundContext.ModerationContext.Message;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.ProcessData;
using Core.Events.ModerationContext;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class AutoModerationOnChapterReadyDomainEventHandler(
    ILogger<AutoModerationOnChapterReadyDomainEventHandler> logger,
    ICache cache,
    IBookApprovalRepository bookApprovalRepository,
    IPlagiarismCheckerService plagiarismCheckerService,
    IVectorEmbeddingTextService vectorEmbeddingTextService)
    : IEventHandler<ChapterReadyForModerationDomainEvent>
{
    private readonly ICache _cache = cache;
    IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    private readonly IVectorEmbeddingTextService _vectorEmbeddingTextService = vectorEmbeddingTextService;
    private readonly IPlagiarismCheckerService _plagiarismCheckerService = plagiarismCheckerService;
    private readonly ILogger<AutoModerationOnChapterReadyDomainEventHandler> _logger = logger;

    public async Task Handler(ChapterReadyForModerationDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start moderation auto {@moderation}", domainEvent.BookApproval);
        var vectorEmbeddings =
            await _vectorEmbeddingTextService.GetVectorEmbeddingFromTextAsync(domainEvent.BookApproval.ContentHash);
        if (vectorEmbeddings is null || !vectorEmbeddings.Any())
        {
            _logger.LogError("Không tạo được vector nhúng cho ContentHash: {ContentHash}", domainEvent.BookApproval.ContentHash);
            return;
        }

        // Ngưỡng tương đồng đoạn (vừa phải, phù hợp viết sách)
        const float similarityThreshold = 0.7f;
        // Ngưỡng tổng tỷ lệ đạo văn (linh hoạt cho viết sách)
        const float plagiarismPercentageThreshold = 35.0f;
        var allMatches = new List<PlagiarismMatch>();
        foreach (var vectorEmbedding in vectorEmbeddings)
        {
            var matches = await
                _plagiarismCheckerService.CheckSimilarityAsync(vectorEmbedding.Embedding.ToArray(),
                    domainEvent.BookApproval.ChapterId.ToString());
            allMatches.AddRange(matches.Where(m => m.Similarity >= similarityThreshold));
        }

        int totalChunks = vectorEmbeddings.Count;
        int similarChunks = allMatches.Count;
        float plagiarismPercentage = (float)similarChunks / totalChunks * 100;
        var sourceDocs = allMatches.GroupBy(m => m.DocId)
            .Select(g => new PlagiarismSource
            {
                DocId = g.Key,
                MatchCount = g.Count(),
                MaxSimilarity = g.Max(m => m.Similarity)
            })
            .OrderByDescending(g => g.MaxSimilarity)
            .ToList();
        bool isPlagiarized = plagiarismPercentage > plagiarismPercentageThreshold || 
                             sourceDocs.Any(s => s.MaxSimilarity > 0.9f);
        if (isPlagiarized)
        {
            string message = ApprovalMessage.GetRejectMessage(plagiarismPercentage, plagiarismPercentageThreshold, similarChunks, totalChunks, sourceDocs);
            domainEvent.BookApproval.Reject(null, message, true);
        }
        else
        {
            domainEvent.BookApproval.Approve(null, ApprovalMessage.ApproveMessage, true);
            await _cache.SetAsync(string.Format(CacheKey.ModerationVectorEmbedding, domainEvent.BookApproval.Id),
                vectorEmbeddings, 1);
        }

        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }

}
    
    
public class PlagiarismSource
{
    public string DocId { get; set; }      
    public int MatchCount { get; set; }         
    public float MaxSimilarity { get; set; }  
}
