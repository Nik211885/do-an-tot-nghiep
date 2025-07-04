using System;
using System.Linq.Expressions;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;

public record GenreViewModel(
    Guid Id,
    string Name,
    string Description,
    string Slug,
    string AvatarUrl,
    bool IsActive,
    int CoutBook,
    DateTimeOffset CreatedDateTime,
    DateTimeOffset LastUpdateDateTime
);


public static class GenreViewModelMappingExtensions
{
    public static GenreViewModel MapToViewModel(this Genres genres)
    {
        return new GenreViewModel(
            Id: genres.Id,
            Name: genres.Name,
            Description: genres.Description,
            Slug: genres.Slug,
            AvatarUrl: genres.AvatarUrl,
            IsActive: genres.IsActive,
            CoutBook: genres.CountBook,
            CreatedDateTime: genres.CreatedDateTime,
            LastUpdateDateTime: genres.LastUpdateDateTime
        );
    }
    public static Expression<Func<Genres, GenreViewModel>> MapToViewModelExpression()
    {
        return genres => new GenreViewModel(
            genres.Id,
            genres.Name,
            genres.Description,
            genres.Slug,
            genres.AvatarUrl,
            genres.IsActive,
            genres.CountBook,
            genres.CreatedDateTime,
            genres.LastUpdateDateTime
        );
    }

    public static IReadOnlyCollection<GenreViewModel> MapToViewModel(this IReadOnlyCollection<Genres> genres)
    {
        return genres.Select(MapToViewModel).ToList();
    }
}
