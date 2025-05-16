
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.BookAuthoringContext.GenresAggregate;

public class Genres : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Slug { get; private set; }
    public string AvatarUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public DateTimeOffset LastUpdateDateTime { get; private set; }
    public IReadOnlyCollection<BookGenres> BookGenres { get; private set; }
    private Genres(string name, string description, string slug, string avatarUrl)
    {
        Name = name;
        Slug = slug;
        AvatarUrl = avatarUrl;
        Description = description;
        CreatedDateTime = DateTimeOffset.UtcNow;
        IsActive = true;
    }

    public static Genres Create(string name, string description, string slug, string avatarUrl)
    {
        return new Genres(name, description, slug, avatarUrl);
    }

    public void Update(string name, string description, string slug)
    {
        Slug = slug;
        Name = name;
        Description = description;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UnActivate()
    {
        IsActive = false;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    public void UpdateAvatar(string avatarUrl)
    {
        AvatarUrl = avatarUrl;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
}
