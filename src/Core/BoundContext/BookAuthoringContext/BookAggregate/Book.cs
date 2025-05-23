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
    public Guid CreatedUerId { get; private set; }
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
    private List<Tag>? _tags;
    public IReadOnlyCollection<Tag>? Tags => _tags?.AsReadOnly();
    private List<Chapter> _chapters;
    public IReadOnlyCollection<Chapter> Chapters => _chapters.AsReadOnly();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="content"></param>
    /// <param name="title"></param>
    /// <param name="chapterSlug"></param>
    public void AddNewChapter(Guid userId, string content, string title, string chapterSlug)
    {
        EnsureOwner(userId);
        var chapter = Chapter.Create(content, title, chapterSlug);
        _chapters.Add(chapter);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="chapterId"></param>
    /// <exception cref="BadRequestException"></exception>

    public void RemoveChapter(Guid userId, Guid chapterId)
    {
        EnsureOwner(userId);
        var chapter = _chapters.FirstOrDefault(x => x.Id == chapterId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(chapter,
            string.Format(BookAuthoringContextMessage.CanNotFindChapterHasId,chapterId));
        _chapters.Remove(chapter);
        RaiseDomainEvent(new RemovedChapterDomainEvent(Id, chapterId));
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     It support for ef constructed
    /// </summary>
    protected Book(){}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createdUserId"></param>
    /// <param name="title"></param>
    /// <param name="avatarUrl"></param>
    /// <param name="description"></param>
    /// <param name="versionNumber"></param>
    /// <param name="readerBookPolicy"></param>
    /// <param name="priceReaderBookPolicy"></param>
    /// <param name="bookReleaseType"></param>
    /// <param name="tagsName"></param>
    /// <param name="visibility"></param>
    /// <param name="genres"></param>
    /// <param name="slug"></param>
    /// <exception cref="BadRequestException"></exception>
    private Book(Guid createdUserId, string title, string? avatarUrl, string? description, 
        int versionNumber, BookPolicy readerBookPolicy, decimal? priceReaderBookPolicy,
        BookReleaseType bookReleaseType,IReadOnlyCollection<string>? tagsName,
        bool visibility, IReadOnlyCollection<Genres> genres,  string slug)
    {
        if(!genres.Any())
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.YourBookMustHasMoreThanOneGenre);
        }
        bool hasDuplicatesGenre = genres
            .GroupBy(x => x.Id)
            .Any(x => x.Count() > 1);
        if (hasDuplicatesGenre)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.DuplicateBookGenre);
        }
        if(tagsName is not null)
        {
            bool hasDuplicateTag = tagsName
                .GroupBy(x => x)
                .Any(x => x.Count() > 1);
            if (hasDuplicateTag)
            {
                ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.DuplicateBookTags);
            }
            _tags ??= [];
            var tags = tagsName.Select(Tag.CreateTag);
            _tags.AddRange(tags);
        }
        CreatedUerId = createdUserId;
        Title = title;
        AvatarUrl = avatarUrl;
        Description = description;
        Slug = slug;
        Visibility = visibility;
        _genres ??= [];
        _genres.AddRange(genres);
        VersionNumber = versionNumber;
        var policyReadBook = PolicyReadBook
            .CreatePolicy(readerBookPolicy, priceReaderBookPolicy);
        PolicyReadBook = policyReadBook;
        BookReleaseType = bookReleaseType;
        IsComplete = false;
        CreatedDateTime = DateTimeOffset.UtcNow;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="policy"></param>
    /// <param name="price"></param>

    public void UpdatePolicyReadBook(Guid userId, BookPolicy policy, decimal? price)
    {
        EnsureOwner(userId);
        var policyReadBook = PolicyReadBook.CreatePolicy(policy, price);
        PolicyReadBook = policyReadBook;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tagName"></param>
    /// <exception cref="BadRequestException"></exception>
    public void AddNewTag(Guid userId, string tagName)
    {
        EnsureOwner(userId);
        var findTagExits = _tags?.Exists(x => x.TagName == tagName) ?? false;
        if (findTagExits)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.TagBookHasExits);
        }
        var tag = Tag.CreateTag(tagName);
        _tags ??= [];
        _tags.Add(tag);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tagName"></param>
    public void RemoveTag(Guid userId, string tagName)
    {
        EnsureOwner(userId);
        var tag = _tags?.FirstOrDefault(x => x.TagName == tagName);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(tag, BookAuthoringContextMessage.TagNotExitsInBook);
        _tags?.Remove(tag);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="title"></param>
    /// <param name="slug"></param>
    public void UpdateTitle(Guid userId, string title, string slug)
    {
        EnsureOwner(userId);
        Title = title;
        Slug = slug;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="avatarUrl"></param>
    public void UpdateAvatarUrl(Guid userId, string avatarUrl)
    {
        EnsureOwner(userId);
        AvatarUrl = avatarUrl;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="description"></param>
    public void UpdateDescription(Guid userId, string description)
    {
        EnsureOwner(userId);
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="visibility"></param>
    public void UpdateVisibility(Guid userId, bool visibility)
    {
        EnsureOwner(userId);
        Visibility = visibility;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="versionNumber"></param>
    public void UpdateVersionNumber(Guid userId, int versionNumber)
    {
        EnsureOwner(userId);
        VersionNumber = versionNumber;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="bookReleaseType"></param>
    public void UpdateBookReleaseType(Guid userId, BookReleaseType bookReleaseType)
    {
        EnsureOwner(userId);
        BookReleaseType = bookReleaseType;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="title"></param>
    /// <param name="avatarUrl"></param>
    /// <param name="description"></param>
    /// <param name="slug"></param>
    public void Update(Guid userId, string title, string? avatarUrl, string? description, string slug)
    {
        EnsureOwner(userId);
        Title = title;
        Slug = slug;
        AvatarUrl = avatarUrl;
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="genres"></param>
    /// <exception cref="BadRequestException"></exception>

    public void AddGenres(Guid userId, Genres genres)
    {
        EnsureOwner(userId);
        var genreExits = _genres.FirstOrDefault(x=>x.Id == genres.Id);
        ThrowHelper.ThrowBadRequestWhenArgumentNotNull(genreExits,BookAuthoringContextMessage.BookCanNotDuplicateGenre);
        _genres.Add(genres);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="genres"></param>
    /// <exception cref="BadRequestException"></exception>
    public void RemoveGenres(Guid userId, Genres genres)
    {
        EnsureOwner(userId);
        if (_genres.Count <= 1)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.CanNotRemoveGenreIsEmpty);
        }
        var genreExits = _genres.FirstOrDefault(x => x.Id == genres.Id);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(genreExits,BookAuthoringContextMessage.CanNotExitsGenresInYourBook);
        _genres.Remove(genres);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createdUserid"></param>
    /// <param name="title"></param>
    /// <param name="avatarUrl"></param>
    /// <param name="description"></param>
    /// <param name="versionNumber"></param>
    /// <param name="readerBookPolicy"></param>
    /// <param name="priceReaderBookPolicy"></param>
    /// <param name="bookReleaseType"></param>
    /// <param name="tagNames"></param>
    /// <param name="visibility"></param>
    /// <param name="genres"></param>
    /// <param name="slug"></param>
    /// <returns></returns>
    public static Book Create(Guid createdUserid, string title, string? avatarUrl, string? description,
        int versionNumber, BookPolicy readerBookPolicy, decimal? priceReaderBookPolicy,
        BookReleaseType bookReleaseType, IReadOnlyCollection<string>? tagNames, bool visibility, 
        IReadOnlyCollection<Genres> genres,string slug)
    {
        return new Book(createdUserid, title, avatarUrl, description, versionNumber, 
            readerBookPolicy,priceReaderBookPolicy,bookReleaseType, tagNames, visibility,genres, slug);
    }
    /// <summary>
    ///  Rule for user can edit and update book
    ///  In here author in book just edit owner book
    /// </summary>
    /// <param name="editUserId">User identifier want edit</param>
    private void EnsureOwner(Guid editUserId)
    {
        if (CreatedUerId != editUserId)
        {
            ThrowHelper.ThrowIfNotOwner();
        }
    }
}
