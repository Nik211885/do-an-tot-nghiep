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
    public string Slug {get; private set;}
    public PolicyReadBook PolicyReadBook { get; private set; }
    public BookReleaseType BookReleaseType { get; private set; }

    private List<Genres> _genres;
    public IReadOnlyCollection<Genres> Genres => _genres.AsReadOnly();
    
    public List<Tag>? Tags { get; private set; }
    private List<Chapter> _chapters;
    public IReadOnlyCollection<Chapter> Chapters => _chapters.AsReadOnly();

    public void AddNewChapter(string content, string title, string chapterSlug)
    {
        var chapter = Chapter.Create(content, title, chapterSlug);
        _chapters.Add(chapter);
    }

    public void RemoveChapter(Guid chapterId)
    {
        var chapter = _chapters.FirstOrDefault(x => x.Id == chapterId);
        if (chapter is null)
        {
            throw new BadRequestException(string.Format(BookAuthoringContextMessage.CanNotFindChapterHasId,chapterId));
        }
        _chapters.Remove(chapter);
        RaiseDomainEvent(new RemovedChapterDomainEvent(Id, chapterId));
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    protected Book(){}
    private Book(string title, string? avatarUrl, string? description, 
        int versionNumber, PolicyReadBook policyReadBook,
        BookReleaseType bookReleaseType,List<Tag>? tags, bool visibility, List<Genres> genres,  string slug)
    {
        Title = title;
        AvatarUrl = avatarUrl;
        Description = description;
        Slug = slug;
        Tags = tags;
        Visibility = visibility;
        _genres = genres;
        VersionNumber = versionNumber;
        PolicyReadBook = policyReadBook;
        BookReleaseType = bookReleaseType;
        IsComplete = false;
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

    public void UpdateTitle(string title, string slug)
    {
        Title = title;
        Slug = slug;
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

    public void Update(string title, string? avatarUrl, string? description, string slug)
    {
        Title = title;
        Slug = slug;
        AvatarUrl = avatarUrl;
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void AddGenres(Genres genres)
    {
        var genreExits = _genres.FirstOrDefault(x=>x.Id == genres.Id);
        if (genreExits is not null)
        {
            throw new BadRequestException(BookAuthoringContextMessage.BookCanNotDuplicateGenre);
        }
        _genres.Add(genres);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void RemoveGenres(Genres genres)
    {
        if (_genres.Count <= 1)
        {
            throw new BadRequestException(BookAuthoringContextMessage.CanNotRemoveGenreIsEmpty);
        }
        var genreExits = _genres.FirstOrDefault(x => x.Id == genres.Id);
        if (genreExits is null)
        {
            throw new BadRequestException(BookAuthoringContextMessage.CanNotExitsGenresInYourBook);
        }
        _genres.Remove(genres);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    public static Book Create(string title, string? avatarUrl, string? description,
        int versionNumber, PolicyReadBook policyReadBook,
        BookReleaseType bookReleaseType, List<Tag>? tags, bool visibility, List<Genres> genres,string slug)
    {
        return new Book(title, avatarUrl, description, versionNumber, 
            policyReadBook,bookReleaseType, tags, visibility,genres, slug);
    }
}
