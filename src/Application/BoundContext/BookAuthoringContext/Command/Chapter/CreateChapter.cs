using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Chapter;

public class CreateChapterCommand() : ICommand<ChapterViewModel>;

public class CreateChapterCommandHandler(IChapterRepository chapterRepository,
    IBookRepository bookRepository,
    ILogger<CreateChapterCommandHandler> logger,
    IIdentityProvider identityProvider) 
    : ICommandHandler<CreateChapterCommand, ChapterViewModel>
{
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly ILogger<CreateChapterCommandHandler> _logger = logger;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public Task<ChapterViewModel> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
