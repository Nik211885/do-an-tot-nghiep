
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.BookAuthoringContext.GenresAggregate;

public class Genres : BaseEntity, IAggregateRoot
{
    public Guid CreateUserId { get; private set; }
    public int CountBook { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Slug { get; private set; }
    public string AvatarUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public DateTimeOffset LastUpdateDateTime { get; private set; }
    protected Genres(){}
    private Genres(Guid createUserId,string name, string description, string slug, string avatarUrl)
    {
        CreateUserId = createUserId;
        CountBook = 0;
        Name = name;
        Slug = slug;
        AvatarUrl = avatarUrl;
        Description = description;
        CreatedDateTime = DateTimeOffset.UtcNow;
        IsActive = true;
    }

    public static Genres Create(Guid createUserId, string name, string description, string slug, string avatarUrl)
    {
        return new Genres(createUserId,name, description, slug, avatarUrl);
    }

    public void Update(string name, string description, string avatarUrl, string slug)
    {
        if (name != Name)
        {
            Slug = slug;
            Name = name;
        }

        AvatarUrl = avatarUrl;
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

    public void AddCoutForBook()
    {
        CountBook++;
    }

    public void RemoveCoutForBook()
    {
        CountBook--;
    }

    public void ChangeActive()
    {
        if (IsActive)
            UnActivate();
        else
            Activate();
    }
}
