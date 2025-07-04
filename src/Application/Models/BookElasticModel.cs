using Application.BoundContext.BookAuthoringContext.ViewModel;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Application.Models;

public class GenreElasticModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public int CoutBook { get; set; }
    public DateTimeOffset CreateDateTime { get; set; }
    public DateTimeOffset LastUpdateTime { get; set; }
}

public class BookElasticModel 
{
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreateDateTimeOffset { get; set; }
    public DateTimeOffset UpdateDateTimeOffset { get; set; }
    public bool IsCompleted { get; set; }
    public bool Visibility { get; set; }
    public string Slug { get; set; }
    public bool IsActive {get; set;}
    public IReadOnlyCollection<string>? Tags { get; set; }
    public decimal? Price { get; set; }
    public BookPolicy BookPolicy { get; set; }
    public BookReleaseType BookReleaseType { get; set; }
    public IEnumerable<GenreElasticModel> Genres { get; set; }
}

public static class BookElasticModelExtension{
    public static GenreElasticModel ToElasticModel(this Genres genre)
    {
        return new GenreElasticModel()
        {
            Id = genre.Id.ToString(),
            AvatarUrl = genre.AvatarUrl,
            Description = genre.Description,
            Slug = genre.Slug,
            CoutBook = genre.CountBook,
            CreateDateTime = genre.CreatedDateTime,
            LastUpdateTime = genre.LastUpdateDateTime,
            IsActive = genre.IsActive,
            Name = genre.Name,
        };
    }

    public static IEnumerable<GenreElasticModel> ToElasticModels(this List<Genres> genres)
    {
        return genres.Select(ToElasticModel);
    }

    public static BookElasticModel ToElasticModel(this Book book,List<Genres> genresList, string authorName)
    {
        return new BookElasticModel()
        {
            AuthorId = book.CreatedUerId.ToString(),
            AuthorName = authorName,
            AvatarUrl = book.AvatarUrl,
            BookPolicy = book.PolicyReadBook.Policy,
            BookReleaseType = book.BookReleaseType,
            CreateDateTimeOffset = book.CreatedDateTime,
            Description = book.Description,
            Genres = genresList.ToElasticModels(),
            Price = book.PolicyReadBook.Price,
            Slug = book.Slug,
            IsActive = false,
            Title = book.Title,
            IsCompleted = book.IsComplete,
            Id = book.Id.ToString(),
            UpdateDateTimeOffset = book.LastUpdateDateTime,
            Tags = book.Tags?.Select(x => x.TagName).ToList(),
            Visibility = book.Visibility,
        };
    }
}
