using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Endpoints;

public class Controller  
    : ControllerBase
{
    private readonly ICommandHandler<CreateBookCommand, BookViewModel> _createBookCommandHandler;
    private readonly ICommandHandler<UpdateBookCommand, BookViewModel> _updateBookCommandHandler;
    private readonly ICommandHandler<DeleteBookCommand, string> _deleteBookCommandHandler;
    private readonly ICommandHandler<ChangeBookReleaseTypeCommand, BookViewModel> _changeBookReleaseTypeCommandHandler;
    private readonly ICommandHandler<ChangePolicyBookCommand, BookViewModel> _changePolicyBookCommandHandler;
    private readonly ICommandHandler<MarkCompletedBookCommand, BookViewModel> _markCompletedBookCommandHandler;

    public Controller(
        ICommandHandler<CreateBookCommand, BookViewModel> createBookCommandHandler,
        ICommandHandler<UpdateBookCommand, BookViewModel> updateBookCommandHandler,
        ICommandHandler<DeleteBookCommand, string> deleteBookCommandHandler,
        ICommandHandler<ChangeBookReleaseTypeCommand, BookViewModel> changeBookReleaseTypeCommandHandler,
        ICommandHandler<ChangePolicyBookCommand, BookViewModel> changePolicyBookCommandHandler,
        ICommandHandler<MarkCompletedBookCommand, BookViewModel> markCompletedBookCommandHandler)
    {
        _createBookCommandHandler = createBookCommandHandler;
        _updateBookCommandHandler = updateBookCommandHandler;
        _deleteBookCommandHandler = deleteBookCommandHandler;
        _changeBookReleaseTypeCommandHandler = changeBookReleaseTypeCommandHandler;
        _changePolicyBookCommandHandler = changePolicyBookCommandHandler;
        _markCompletedBookCommandHandler = markCompletedBookCommandHandler;
    }
}
