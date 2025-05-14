
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.BookAuthoringContext.GenresAggregate;

public class Genres : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public DateTimeOffset LastUpdateDateTime { get; private set; }
    public IReadOnlyCollection<BookGenres> BookGenres { get; private set; }
    private Genres(string name, string description)
    {
        Name = name;
        Description = description;
        CreatedDateTime = DateTimeOffset.UtcNow;
        IsActive = true;
    }

    public static Genres Create(string name, string description)
    {
        return new Genres(name, description);
    }

    public void Update(string name, string description)
    {
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
}
