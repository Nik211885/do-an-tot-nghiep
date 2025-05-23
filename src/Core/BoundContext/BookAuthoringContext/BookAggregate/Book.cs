using Core.Entities;
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
    
    private List<BookGenres> _genres;
    public IReadOnlyCollection<BookGenres> Genres => _genres.AsReadOnly();
    private List<Tag>? _tags;
    public IReadOnlyCollection<Tag>? Tags => _tags?.AsReadOnly();
    /// <summary>
    ///     It support for ef constructed
    /// </summary>
    protected Book(){}
    /// <summary>
    ///     Constructed create new instance for book and follow rule domain
    ///     is don't have any tags or genre duplicate name and id
    ///     and rule create policy reader book
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
    /// <param name="genreIds"></param>
    /// <param name="slug"></param>
    /// <exception cref="BadRequestException"></exception>
    private Book(Guid createdUserId, string title, string? avatarUrl, string? description, 
        int versionNumber, BookPolicy readerBookPolicy, decimal? priceReaderBookPolicy,
        BookReleaseType bookReleaseType,IReadOnlyCollection<string>? tagsName,
        bool visibility, IReadOnlyCollection<Guid> genreIds,  string slug)
    {
        if(!genreIds.Any())
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.YourBookMustHasMoreThanOneGenre);
        }
        bool hasDuplicatesGenre = genreIds
            .GroupBy(x => x)
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
        _genres.AddRange(genreIds.Select(BookGenres.Create));
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
    ///    Update policy reader book  and follow policy factory rule 
    /// </summary>
    /// <param name="userId">It rule ensure author just have change book</param>
    /// <param name="policy">Policy in book</param>
    /// <param name="price">Price when policy is paid</param>

    public void UpdatePolicyReadBook(Guid userId, BookPolicy policy, decimal? price)
    {
        EnsureOwner(userId);
        var policyReadBook = PolicyReadBook.CreatePolicy(policy, price);
        PolicyReadBook = policyReadBook;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///    Add new tag and follower rule factory create tag and don't have
    ///     any tag have name is duplicate
    /// </summary>
    /// <param name="userId">It rule ensure author just have change book</param>
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
    ///     Remove tag for book and make sure tag name exits in book
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
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
    ///     Update title for book, and it just has change when title
    ///     has changed in any character it will make slug changed
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="title"></param>
    /// <param name="slug"></param>
    public void UpdateTitle(Guid userId, string title, string slug)
    {
        EnsureOwner(userId);
        if (title != Title)
        {
            Title = title;
            Slug = slug;
        }
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Update avatar image for book
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="avatarUrl"></param>
    public void UpdateAvatarUrl(Guid userId, string avatarUrl)
    {
        EnsureOwner(userId);
        AvatarUrl = avatarUrl;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Update description for book
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="description"></param>
    public void UpdateDescription(Guid userId, string description)
    {
        EnsureOwner(userId);
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Change visibility for book
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="visibility"></param>
    public void UpdateVisibility(Guid userId, bool visibility)
    {
        EnsureOwner(userId);
        Visibility = visibility;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Update version number for book
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="versionNumber"></param>
    public void UpdateVersionNumber(Guid userId, int versionNumber)
    {
        EnsureOwner(userId);
        VersionNumber = versionNumber;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Update book release type for book
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="bookReleaseType"></param>
    public void UpdateBookReleaseType(Guid userId, BookReleaseType bookReleaseType)
    {
        EnsureOwner(userId);
        BookReleaseType = bookReleaseType;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Update information for book and when title has changed slug just has  change
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="title"></param>
    /// <param name="avatarUrl"></param>
    /// <param name="description"></param>
    /// <param name="slug"></param>
    public void Update(Guid userId, string title, string? avatarUrl, string? description, string slug)
    {
        EnsureOwner(userId);
        if (Title != title)
        {
            Title = title;
            Slug = slug;
        }

        AvatarUrl = avatarUrl;
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Add new genre for book and one genre just has constance one book
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="genreId"></param>
    /// <exception cref="BadRequestException"></exception>

    public void AddGenres(Guid userId, Guid genreId)
    {
        EnsureOwner(userId);
        var genreExits = _genres.FirstOrDefault(x=>x.GenreId == genreId);
        ThrowHelper.ThrowBadRequestWhenArgumentNotNull(genreExits,BookAuthoringContextMessage.BookCanNotDuplicateGenre);
        _genres.Add(BookGenres.Create(genreId));
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Remove genre for book make sure it have more than one
    ///     genre and genre id has exits in genres
    /// </summary>
    /// <param name="userId">it rule ensure author just have change book</param>
    /// <param name="genreId"></param>
    /// <exception cref="BadRequestException"></exception>
    public void RemoveGenres(Guid userId, Guid genreId)
    {
        EnsureOwner(userId);
        if (_genres.Count <= 1)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.CanNotRemoveGenreIsEmpty);
        }
        var genreExits = _genres.FirstOrDefault(x => x.GenreId == genreId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(genreExits,BookAuthoringContextMessage.CanNotExitsGenresInYourBook);
        _genres.Remove(genreExits);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///    Factory create new instance book with constructed primary
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
    /// <param name="genresId"></param>
    /// <param name="slug"></param>
    /// <returns></returns>
    public static Book Create(Guid createdUserid, string title, string? avatarUrl, string? description,
        int versionNumber, BookPolicy readerBookPolicy, decimal? priceReaderBookPolicy,
        BookReleaseType bookReleaseType, IReadOnlyCollection<string>? tagNames, bool visibility, 
        IReadOnlyCollection<Guid> genresId,string slug)
    {
        return new Book(createdUserid, title, avatarUrl, description, versionNumber, 
            readerBookPolicy,priceReaderBookPolicy,bookReleaseType, tagNames, visibility,genresId, slug);
    }
    /// <summary>
    ///  Rule for user can edit and update book
    ///  In here author in book just edit owner book
    /// </summary>
    /// <param name="editUserId">User identifier want edit</param>
    public void EnsureOwner(Guid editUserId)
    {
        if (CreatedUerId != editUserId)
        {
            ThrowHelper.ThrowIfNotOwner();
        }
    }
}
