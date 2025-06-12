using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.Validator;
using Application.Models;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Events.ModerationContext;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class ActiveBookChapterApprovedSuccessDomainEventHandler(
    IElasticFactory elasticFactory,
    ILogger<ActiveBookChapterApprovedSuccessDomainEventHandler> logger,
    IValidationServices<BookApproval> bookApprovalValidationServices)
    : IEventHandler<ApprovedBookDomainEvent>
{
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    private readonly IValidationServices<BookApproval>  _bookApprovalValidationServices = bookApprovalValidationServices;
    private readonly ILogger<ActiveBookChapterApprovedSuccessDomainEventHandler> _logger = logger;
    public async Task Handler(ApprovedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookApprovalExits = await _bookApprovalValidationServices.AnyAsync(
            s => s.BookId == domainEvent.Approval.BookId 
                 && s.Status == BookApprovalStatus.Approved, cancellationToken);

        if (bookApprovalExits is not null)
        {
            _logger.LogInformation("Book has active befor {@Id}", domainEvent.Approval.BookId);
            return;
            
        }

        var bookApprovalElastic = _elasticFactory.GetInstance<BookElasticModel>();
        
        var bookInElastic = await bookApprovalElastic.GetAsync(domainEvent.Approval.BookId);
        if (bookInElastic is not null)
        {
            bookInElastic.IsActive = true;  
            await bookApprovalElastic.UpdateAsync(bookInElastic);
            _logger.LogInformation("Book approved successfully");
        }
        // Create new instance for book
    }
}
