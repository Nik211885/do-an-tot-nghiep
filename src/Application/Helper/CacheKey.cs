namespace Application.Helper;

public static class CacheKey
{
    // prefix
    public const string Chapter = "app:book-authoring:chapter-";
    
    // cache key with 0 is chapter id and 1 is chapter version need rollback
    public static readonly string ChapterRollBack = $"{Chapter}:rollback:{0}:{1}";
}
