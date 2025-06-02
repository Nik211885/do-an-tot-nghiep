namespace Application.Helper;

public static class CacheKey
{
    // prefix
    public const string Chapter = "app:book-authoring:chapter-";
    public const string Genre = "app:book-authoring:genre";
    public const string Book = "app:book-authoring:book";
    public const string Moderation = "app:book-authoring:moderator";
    
    // cache key with 0 is chapter id and 1 is chapter version need rollback
    public static readonly string ChapterRollBack = Chapter + ":rollback:{0}:{1}";
    public static readonly string BookByUserId = Book + ":by-user:{0}";
    public static readonly string ChapterByBookSlug = Chapter +":chapter-by-book:{0}";
    
    public static readonly string ModerationVectorEmbedding = Moderation + ":vector-embedding{0}";
}
