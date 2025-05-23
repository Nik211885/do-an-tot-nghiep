﻿using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;


public class BookViewModel(Guid authorId, Guid id, string title, string? avatarUrl,
    string? description, DateTimeOffset createDateTimeOffset, DateTimeOffset lastUpdateDateTime,
    bool isComplete, int versionNumber, bool visibility, string slug,
    PolicyReadBook policyReadBook, BookReleaseType bookReleaseType, 
    IReadOnlyCollection<Tag>? tags, IReadOnlyCollection<Guid> genreIds)
{
    public Guid AuthorId { get; }  = authorId;
    public Guid Id { get; } = id;
    public string Title { get; }  = title;
    public string? AvatarUrl { get; } = avatarUrl;
    public string? Description { get; } = description;
    public DateTimeOffset CreateDateTimeOffset { get; } = createDateTimeOffset;
    public DateTimeOffset LastUpdateDateTime { get; } = lastUpdateDateTime;
    public bool IsComplete { get; } = isComplete;
    public int VersionNumber { get; } = versionNumber;
    public bool Visibility { get; } = visibility;
    public string Slug { get; } = slug;
    public PolicyReadBook PolicyReadBook { get; } = policyReadBook;
    public BookReleaseType BookReleaseType { get; } = bookReleaseType;
    public IReadOnlyCollection<Tag>? Tags { get; } = tags;
    public IReadOnlyCollection<Guid> GenreIds { get; } = genreIds;
}

public static class BookViewModelMappingExtension
{
    public static BookViewModel MapToViewModel(this Book book)
    {
        return new BookViewModel(
            authorId: book.CreatedUerId,
            id: book.Id, 
            title: book.Title, 
            avatarUrl: book.AvatarUrl,
            description: book.Description, 
            createDateTimeOffset: book.CreatedDateTime,
            lastUpdateDateTime: book.LastUpdateDateTime,
            isComplete: book.IsComplete,
            versionNumber: book.VersionNumber,
            visibility: book.Visibility,
            slug: book.Slug, 
            policyReadBook: book.PolicyReadBook, 
            bookReleaseType: book.BookReleaseType,
            tags: book.Tags, 
            genreIds: book.Genres.Select(x=>x.GenreId).ToList());
    }
}
