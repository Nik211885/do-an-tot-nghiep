﻿using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookAuthoringContext;

public class BookRepository(BookAuthoringDbContext bookAuthoringDbContext) 
    : Repository<Book>(bookAuthoringDbContext), IBookRepository
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
    public async Task<Book?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var bookById = await _bookAuthoringDbContext.Books
            .Where(book => book.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return bookById;
    }

    public Book Create(Book book)
    {
        return _bookAuthoringDbContext.Books.Add(book).Entity;
    }

    public Book Update(Book book)
    {
        _bookAuthoringDbContext.Books.Update(book);
        return book;
    }
}
