using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.Entities;
using Core.Events.WriteBookContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.BookAuthoringContext.BookAggregate;

public class Book 
    : BaseEntity, IAggregateRoot
{
    public string Title { get; private set; } 
    public string? AvatarUrl { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset CreatedDateTime { get;private set; }
    public DateTimeOffset LastUpdateDateTime { get; private set; }
    public bool IsComplete { get; private set; }
    public int VersionNumber { get; private set; }
    public bool Visibility { get; private set; }
    // public string Slug {get; private set;}
    public PolicyReadBook PolicyReadBook { get; private set; }
    public BookReleaseType BookReleaseType { get; private set; }
    
    private List<BookGenres> _genres;
    public IReadOnlyCollection<BookGenres> GenreIds => _genres.AsReadOnly();
    public List<Tag>? Tags { get; private set; }
    private List<Chapter> _chapters;
    public IReadOnlyCollection<Chapter> Chapters => _chapters.AsReadOnly();

    public void AddNewChapter(string content, string title)
    {
        var chapter = Chapter.Create(Id, content, title);
        _chapters ??= [];
        _chapters.Add(chapter);
    }

    public void RemoveChapter(Guid chapterId)
    {
        var chapter = _chapters?.FirstOrDefault(x => x.Id == chapterId);
        if (chapter is null)
        {
            throw new BadRequestException(string.Format(BookAuthoringContextMessage.CanNotFindChapterHasId,chapterId));
        }
        _chapters?.Remove(chapter);
        RaiseDomainEvent(new RemovedChapterDomainEvent(Id, chapterId));
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    private Book(string title, string? avatarUrl, string? description, 
        int versionNumber, PolicyReadBook policyReadBook, 
        BookReleaseType bookReleaseType,List<Tag>? tags, bool visibility,
        List<Guid> genres)
    {
        Title = title;
        AvatarUrl = avatarUrl;
        Description = description;
        Visibility = visibility;
        VersionNumber = versionNumber;
        PolicyReadBook = policyReadBook;
        BookReleaseType = bookReleaseType;
        IsComplete = false;
        _genres = genres.Select(gId => BookGenres.Create(Id, gId)).ToList();
        CreatedDateTime = DateTimeOffset.UtcNow;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdatePolicyReadBook(BookPolicy policy, decimal? price)
    {
        var policyReadBook = PolicyReadBook.CreatePolicy(policy, price);
        PolicyReadBook = policyReadBook;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void AddNewTag(string tagName)
    {
        var findTagExits = Tags?.Exists(x => x.TagName == tagName) ?? false;
        if (findTagExits)
        {
            throw new BadRequestException(BookAuthoringContextMessage.TagBookHasExits);
        }
        var tag = Tag.CreateTag(tagName);
        Tags ??= [];
        Tags.Add(tag);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void RemoveTag(string tagName)
    {
        var tag = Tags?.FirstOrDefault(x => x.TagName == tagName);
        if (tag is null)
        {
            return;
        }
        Tags?.Remove(tag);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdateTitle(string title)
    {
        Title = title;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdateAvatarUrl(string avatarUrl)
    {
        AvatarUrl = avatarUrl;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdateVisibility(bool visibility)
    {
        Visibility = visibility;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdateVersionNumber(int versionNumber)
    {
        VersionNumber = versionNumber;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdateBookReleaseType(BookReleaseType bookReleaseType)
    {
        BookReleaseType = bookReleaseType;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void Update(string title, string? avatarUrl, string? description)
    {
        Title = title;
        AvatarUrl = avatarUrl;
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void AddGenres(Guid genreId)
    {
        var genreExits = _genres.FirstOrDefault(x=>x.GenreId == genreId && x.BookId == Id);
        if (genreExits is not null)
        {
            throw new BadRequestException(BookAuthoringContextMessage.BookCanNotDuplicateGenre);
        }
        _genres.Add(BookGenres.Create(genreId, Id));
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void RemoveGenres(Guid genreId)
    {
        if (_genres.Count <= 1)
        {
            throw new BadRequestException(BookAuthoringContextMessage.CanNotRemoveGenreIsEmpty);
        }
        var genreExits = _genres.FirstOrDefault(x => x.GenreId == genreId && x.BookId == Id);
        if (genreExits is null)
        {
            throw new BadRequestException(BookAuthoringContextMessage.CanNotExitsGenresInYourBook);
        }
        _genres.Add(BookGenres.Create(genreId, Id));
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    public static Book Create(string title, string? avatarUrl, string? description,
        int versionNumber, PolicyReadBook policyReadBook,
        BookReleaseType bookReleaseType, List<Tag>? tags, bool visibility, List<Guid> genres)
    {
        return new Book(title, avatarUrl, description, versionNumber, 
            policyReadBook,bookReleaseType, tags, visibility, genres);
    }
}
