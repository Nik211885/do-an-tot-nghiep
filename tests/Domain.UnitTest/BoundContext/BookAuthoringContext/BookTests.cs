using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Domain.UnitTest.BoundContext.BookAuthoringContext;

public class BookTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var title = "Book Title";
        var avatar = "avatar.png";
        var desc = "desc";
        var policy = BookPolicy.Free;
        decimal? price = null;
        var releaseType = BookReleaseType.Serialized;
        var tags = new List<string> { "tag1", "tag2" };
        var genres = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var slug = "book-title";

        // Act
        var book = Book.Create(userId, title, avatar, desc, policy, price, releaseType, tags, genres, slug);

        // Assert
        Assert.Equal(title, book.Title);
        Assert.Equal(userId, book.CreatedUerId);
        Assert.Equal(avatar, book.AvatarUrl);
        Assert.Equal(desc, book.Description);
        Assert.Equal(slug, book.Slug);
        Assert.Equal(policy, book.PolicyReadBook.Policy);
        Assert.Equal(releaseType, book.BookReleaseType);
        Assert.Equal(2, book.Genres.Count);
        Assert.Equal(2, book.Tags.Count);
    }

    [Fact]
    public void Create_Should_Throw_When_Duplicate_Genre()
    {
        var userId = Guid.NewGuid();
        var title = "Book Title";
        var genres = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        genres[2] = genres[1]; // duplicate
        // Act & Assert
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() =>
            Book.Create(userId, title, null, null, BookPolicy.Free, null, BookReleaseType.Serialized, null, genres, "slug"));
        Assert.Contains("DuplicateBookGenre", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_No_Genre()
    {
        var userId = Guid.NewGuid();
        // Act & Assert
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() =>
            Book.Create(userId, "Book Title", null, null, BookPolicy.Free, null, BookReleaseType.Serialized, null, new List<Guid>(), "slug"));
        Assert.Contains("YourBookMustHasMoreThanOneGenre", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_Duplicate_Tag()
    {
        var userId = Guid.NewGuid();
        var genres = new List<Guid> { Guid.NewGuid() };
        var tags = new List<string> { "tag1", "tag1" };
        // Act & Assert
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() =>
            Book.Create(userId, "Book Title", null, null, BookPolicy.Free, null, BookReleaseType.Serialized, tags, genres, "slug"));
        Assert.Contains("DuplicateBookTags", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_PaidPolicy_Without_Price()
    {
        var userId = Guid.NewGuid();
        var genres = new List<Guid> { Guid.NewGuid() };
        // Act & Assert
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() =>
            Book.Create(userId, "Book Title", null, null, BookPolicy.Paid, null, BookReleaseType.Serialized, null, genres, "slug"));
        Assert.Contains("BookPaidPolicyButDontAddPrice", ex.Message);
    }

    [Fact]
    public void Create_Should_Throw_When_FreePolicy_With_Price()
    {
        var userId = Guid.NewGuid();
        var genres = new List<Guid> { Guid.NewGuid() };
        // Act & Assert
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() =>
            Book.Create(userId, "Book Title", null, null, BookPolicy.Free, 1000, BookReleaseType.Serialized, null, genres, "slug"));
        Assert.Contains("BookNotPaidPolicyButAddPrice", ex.Message);
    }
}
