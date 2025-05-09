using Core.Entities;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.WriteBookContext.BookAggregate;

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
    public PolicyReadBook PolicyReadBook { get; private set; }
    public BookReleaseType BookReleaseType { get; private set; }
    
    private List<Guid> _genreIds;
    public IReadOnlyCollection<Guid> GenreIds => _genreIds.AsReadOnly();
    public List<Tag>? Tags { get; private set; }

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
        Tags = tags;
        _genreIds = genres;
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
            throw new BadRequestException(BookManagementContextMessage.TagBookHasExits);
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
        _genreIds.Add(genreId);
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void RemoveGenres(Guid genreId)
    {
        if (_genreIds.Count <= 1)
        {
            throw new BadRequestException(BookManagementContextMessage.CanNotRemoveGenreIsEmpty);
        }
        _genreIds.Remove(genreId);
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
