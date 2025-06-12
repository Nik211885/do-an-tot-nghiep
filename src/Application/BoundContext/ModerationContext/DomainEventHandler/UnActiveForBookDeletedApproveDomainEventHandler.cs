using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.Validator;
using Application.Models;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Events.ModerationContext;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class UnActiveForBookDeletedApproveDomainEventHandler(
    ILogger<UnActiveForBookDeletedApproveDomainEventHandler> logger,
    IBookApprovalRepository bookApprovalRepository,
    IElasticFactory elasticFactory,
    IValidationServices<BookApproval> bookApprovalValidationServices)
    : IEventHandler<DeletedApproveDomainEvent>
{
    private readonly ILogger<UnActiveForBookDeletedApproveDomainEventHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    private readonly IValidationServices<BookApproval> _bookApprovalValidationServices = bookApprovalValidationServices;
    public async Task Handler(DeletedApproveDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var coutByBook =
            await _bookApprovalValidationServices.CoutAsync(x => x.BookId == domainEvent.BookApproval.BookId, cancellationToken);
        if (coutByBook > 1)
        {
            _logger.LogInformation("Find has more chapter for book has approved don't unactive for book {@Id}", domainEvent.BookApproval.BookId);
            return;
        }

        var bookApproved =
            await _bookApprovalRepository.FindByBookIdAsync(domainEvent.BookApproval.Id, cancellationToken);
        var bookApprovedFirst = bookApproved.FirstOrDefault();
        // If just have one approved for each chapter in book i will turn off it
        if (bookApprovedFirst is null 
            || bookApprovedFirst.ChapterId == domainEvent.BookApproval.ChapterId)
        {
            var bookElastic = _elasticFactory.GetInstance<BookElasticModel>();
            var bookUnActive = await bookElastic.GetAsync(domainEvent.BookApproval.BookId.ToString());
            if (bookUnActive is null)
            {
                _logger.LogInformation("Can't not find book in elastic {@BookId}", domainEvent.BookApproval.BookId);
                return;
            }
            bookUnActive.IsActive = false;
            await bookElastic.UpdateAsync(bookUnActive);
        }
    }
}

