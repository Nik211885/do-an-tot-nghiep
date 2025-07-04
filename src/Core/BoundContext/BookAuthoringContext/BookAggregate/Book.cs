using Core.Entities;
using Core.Events.BookAuthoringContext;
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
    /*public int VersionNumber { get; private set; }*/
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
    ///     Constructed create new instance for book 
    /// </summary>
    /// <param name="createdUserId"></param>
    /// <param name="title"></param>
    /// <param name="avatarUrl"></param>
    /// <param name="description"></param>
    /// <param name="readerBookPolicy"></param>
    /// <param name="priceReaderBookPolicy"></param>
    /// <param name="bookReleaseType"></param>
    /// <param name="tags"></param>
    /// <param name="genres"></param>
    /// <param name="slug"></param>
    /// <exception cref="BadRequestException"></exception>
    private Book(Guid createdUserId, string title, string? avatarUrl, string? description, 
        BookPolicy readerBookPolicy, decimal? priceReaderBookPolicy,
        BookReleaseType bookReleaseType,IReadOnlyCollection<Tag>? tags, IReadOnlyCollection<BookGenres> genres,  string slug)
    {
        CreatedUerId = createdUserId;
        Title = title;
        AvatarUrl = avatarUrl;
        Description = description;
        Slug = slug;
        Visibility = true;
        _tags ??= [];
        _tags.AddRange(tags ?? []);
        _genres ??= [];
        _genres.AddRange(genres);
        /*VersionNumber = versionNumber;*/
        var policyReadBook = PolicyReadBook
            .CreatePolicy(readerBookPolicy, priceReaderBookPolicy);
        PolicyReadBook = policyReadBook;
        BookReleaseType = bookReleaseType;
        IsComplete = false;
        CreatedDateTime = DateTimeOffset.UtcNow;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new CreatedBookDomainEvent(this, createdUserId));
    }

    /// <summary>
    ///    Update policy reader book  and follow policy factory rule 
    /// </summary>
    /// <param name="policy">Policy in book</param>
    /// <param name="price">Price when policy is paid</param>
    public void Delete()
    {
        // Like chapter
        RaiseDomainEvent(new DeletedBookDomainEvent(this, CreatedUerId));
    }
    public void MarkCompletedBook()
    {
        this.IsComplete = !this.IsComplete;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    public void UpdatePolicyReadBook(BookPolicy policy, decimal? price)
    {
        var policyReadBook = PolicyReadBook.CreatePolicy(policy, price);
        PolicyReadBook = policyReadBook;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new BookUpdatePolicyReaderBookDomainEvent(this,Id, policyReadBook,CreatedUerId));
    }
    /// <summary>
    ///    Add new tag and follower rule factory create tag and don't have
    ///     any tag have name is duplicate
    /// </summary>
    /// <param name="tagName"></param>
    /// <exception cref="BadRequestException"></exception>
    public void AddNewTag(string tagName)
    {
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
    /// <param name="tagName"></param>
    public void RemoveTag(string tagName)
    {
        var tag = _tags?.FirstOrDefault(x => x.TagName == tagName);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(tag, BookAuthoringContextMessage.TagNotExitsInBook);
        _tags?.Remove(tag);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Update title for book, and it just has change when title
    ///     has changed in any character it will make slug changed
    /// </summary>
    /// <param name="title"></param>
    /// <param name="slug"></param>
    public void UpdateTitle(string title, string slug)
    {
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
    /// <param name="avatarUrl"></param>
    public void UpdateAvatarUrl(string avatarUrl)
    {
        AvatarUrl = avatarUrl;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Update description for book
    /// </summary>
    /// <param name="description"></param>
    public void UpdateDescription(string description)
    {
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     Change visibility for book
    /// </summary>
    /// <param name="visibility"></param>
    public void UpdateVisibility( bool visibility)
    {
        Visibility = visibility;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }/*
    /// <summary>
    ///     Update version number for book
    /// </summary>
    /// <param name="versionNumber"></param>
    public void UpdateVersionNumber(int versionNumber)
    {
        VersionNumber = versionNumber;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }*/
    /// <summary>
    ///     Update book release type for book
    /// </summary>
    /// <param name="bookReleaseType"></param>
    public void UpdateBookReleaseType(BookReleaseType bookReleaseType)
    {
        BookReleaseType = bookReleaseType;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new BookChangedReleaseTypeDomainEvent(this,Id,  bookReleaseType,CreatedUerId));
    }

    /// <summary>
    ///     Update information for book and when title has changed slug just has  change
    /// </summary>
    /// <param name="title"></param>
    /// <param name="avatarUrl"></param>
    /// <param name="description"></param>
    /// <param name="slug"></param>
    /// <param name="bookPolicy"></param>
    /// <param name="priceReaderBook"></param>
    /// <param name="visibility"></param>
    /// <param name="releaseType"></param>
    /// <param name="tagNames"></param>
    /// <param name="genreIds"></param>
    public void Update( string title, string? avatarUrl, string? description, 
        string slug, BookPolicy bookPolicy, decimal? priceReaderBook, bool visibility,
        BookReleaseType releaseType,  IReadOnlyCollection<string>? tagNames, 
        IReadOnlyCollection<Guid> genreIds)
    {
        if (!genreIds.Any())
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.YourBookMustHasMoreThanOneGenre);
        }
        if (HasDuplicates(genreIds))
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.DuplicateBookGenre);
        }
        
        var newGenreIds = new HashSet<Guid>(genreIds);
        var oldGenreIds = new HashSet<Guid>(_genres.Select(g => g.GenreId));
        
        var removedGenres = _genres.Where(g => !newGenreIds.Contains(g.GenreId)).ToArray();
        
        var addedGenreIds = newGenreIds.Except(oldGenreIds).ToList();
        var addedGenres = addedGenreIds.Select(BookGenres.Create).ToArray();   
        
        _genres.RemoveAll(g => removedGenres.Select(x=>x).Contains(g));
        RaiseDomainEvent(new RemovedGenreForBookDomainEvent(this, Id,  removedGenres));
        _genres.AddRange(addedGenres);
        RaiseDomainEvent(new AddedGenreForBookDomainEvent(this, Id,addedGenres));
        
        if (tagNames is not null)
        {
            if (HasDuplicates(tagNames))
            {
                ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.DuplicateBookTags);
            }

            _tags = tagNames.Select(Tag.CreateTag).ToList();
        }
        if (Title != title)
        {
            Title = title;
            Slug = slug;
        }
        PolicyReadBook = PolicyReadBook.CreatePolicy(bookPolicy, priceReaderBook);
        BookReleaseType = releaseType;
        Visibility = visibility;
        AvatarUrl = avatarUrl;
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new UpdatedBookDomainEvent(this,CreatedUerId));
    }
    /// <summary>
    ///     Add new genre for book and one genre just has constance one book
    /// </summary>
    /// <param name="genreId"></param>
    /// <exception cref="BadRequestException"></exception>

    public void AddGenres(Guid genreId)
    {
        var genreExits = _genres.FirstOrDefault(x=>x.GenreId == genreId);
        ThrowHelper.ThrowBadRequestWhenArgumentNotNull(genreExits,BookAuthoringContextMessage.BookCanNotDuplicateGenre);
        var genreAdd = BookGenres.Create(genreId);
        _genres.Add(genreAdd);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new AddedGenreForBookDomainEvent(this,Id, genreAdd));
    }
    /// <summary>
    ///     Remove genre for book make sure it have more than one
    ///     genre and genre id has exits in genres
    /// </summary>
    ///  <param name="genreId"></param>
    /// <exception cref="BadRequestException"></exception>
    public void RemoveGenres(Guid genreId)
    {
        if (_genres.Count <= 1)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.CanNotRemoveGenreIsEmpty);
        }
        var genreExits = _genres.FirstOrDefault(x => x.GenreId == genreId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(genreExits,BookAuthoringContextMessage.CanNotExitsGenresInYourBook);
        _genres.Remove(genreExits);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new RemovedGenreForBookDomainEvent(this,Id,genreExits));
    }
    /// <summary>
    ///    Factory create new instance book with constructed primary
    ///     Create new instance for book and follow rule domain
    ///     is don't have any tags or genre duplicate name and id
    ///     and rule create policy reader book
    /// </summary>
    /// <param name="createdUserid"></param>
    /// <param name="title"></param>
    /// <param name="avatarUrl"></param>
    /// <param name="description"></param>
    /// <param name="readerBookPolicy"></param>
    /// <param name="priceReaderBookPolicy"></param>
    /// <param name="bookReleaseType"></param>
    /// <param name="tagNames"></param>
    /// <param name="genreIds"></param>
    /// <param name="slug"></param>
    /// <returns></returns>
    public static Book Create(Guid createdUserid, string title, string? avatarUrl, string? description,
        BookPolicy readerBookPolicy, decimal? priceReaderBookPolicy,
        BookReleaseType bookReleaseType, IReadOnlyCollection<string>? tagNames,
        IReadOnlyCollection<Guid> genreIds,string slug)
    {
        if (!genreIds.Any())
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.YourBookMustHasMoreThanOneGenre);
        }
        if (HasDuplicates(genreIds))
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.DuplicateBookGenre);
        }
        var genres = genreIds.Select(BookGenres.Create).ToList();
        List<Tag> tags = [];
        if (tagNames is not null)
        {
            if (HasDuplicates(tagNames))
            {
                ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.DuplicateBookTags);
            }

            tags = tagNames.Select(Tag.CreateTag).ToList();
        }

        return new Book(createdUserid, title, avatarUrl, description,
            readerBookPolicy, priceReaderBookPolicy, bookReleaseType, tags, genres, slug);
        
    }
    static bool HasDuplicates<T>(IEnumerable<T> source) =>
        source.GroupBy(x => x).Any(g => g.Count() > 1);
}
