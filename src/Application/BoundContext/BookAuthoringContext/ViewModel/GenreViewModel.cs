using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;

public class GenreViewModel(Guid id, 
    string name,
    string description, 
    string slug,
    string avatarUrl,
    bool isActive, 
    int countBook,
    DateTimeOffset createDateTime,
    DateTimeOffset lastUpdateDateTime)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string Slug { get; }  = slug;
    public string AvatarUrl { get; } = avatarUrl;
    public bool IsActive { get; }  = isActive;
    public DateTimeOffset CreatedDateTime { get; } = createDateTime;
    public int CoutBook { get; } = countBook;
    public DateTimeOffset LastUpdateDateTime { get; } = lastUpdateDateTime;
}

public static class GenreViewModelMappingExtensions
{
    public static GenreViewModel MapToViewModel(this Genres genres)
    {
        return new GenreViewModel(
            id: genres.Id,
            name: genres.Name,
            description: genres.Description,
            slug: genres.Slug,
            avatarUrl: genres.AvatarUrl,
            isActive: genres.IsActive,
            countBook:genres.CountBook,
            createDateTime: genres.CreatedDateTime,
            lastUpdateDateTime: genres.LastUpdateDateTime
        );
    }

    public static IReadOnlyCollection<GenreViewModel> MapToViewModel(this IReadOnlyCollection<Genres> genres)
    {
        return genres.Select(MapToViewModel).ToList();
    }
}
