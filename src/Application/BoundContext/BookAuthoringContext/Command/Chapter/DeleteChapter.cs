using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.Validator;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Chapter;

public record DeleteChapterCommand(Guid Id)
    : IBookAuthoringCommand<string>;

public class DeleteChapterCommandHandler(
    ILogger<DeleteChapterCommandHandler> logger,
    IChapterRepository chapterRepository,
    IEventDispatcher eventDispatcher)
    : ICommandHandler<DeleteChapterCommand, string>
{
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    private readonly ILogger<DeleteChapterCommandHandler> _logger = logger;
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;
    public async Task<string> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _chapterRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, "Chương");
        chapter.Delete();
        var domainEvent = chapter.DomainEvents;
        _chapterRepository.Delete(chapter);
        _logger.LogInformation("Delete chapter has id {@Id}", chapter.Id);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        if(domainEvent != null)
            await _eventDispatcher.Dispatch(domainEvent, cancellationToken);
        _logger.LogInformation("Delete chapter id: {@Id} has success", chapter.Id);
        return chapter.Id.ToString();
    }
}
