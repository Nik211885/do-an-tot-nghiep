using Core.Entities;

namespace Core.BoundContext.BookAuthoringContext.ChapterAggregate;

public class ChapterVersion : BaseEntity
{
    /// <summary>
    /// It make use rembember what name is history
    /// </summary>
    ///
    // Shadow prop id chapter
    public string NameVersion { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public string DiffContent { get; private set; }
    public uint Version { get; private set; }
    public string DiffTitle { get; private set; }

    /*public string ContentSnapShot { get; private set; }*/
    /*public string TitleSnapShot { get;private set; }*/
    //public bool IsSnapShot { get; private set; }
    /*public bool IsRollBack { get; private set; }*/
    protected ChapterVersion(){}
    /// <summary>
    ///    
    /// </summary>
    /// <param name="nameVersion"></param>
    /// <param name="diffContent"></param>
    /// <param name="diffTitle"></param>
    /// <param name="version"></param>
    private ChapterVersion(string nameVersion, string diffContent, string diffTitle, uint version)
    {
        NameVersion = nameVersion;
        DiffTitle = diffTitle;
        DiffContent = diffContent;
        Version = version;
        CreatedDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    ///     If use don't fact change anything  i will don't apply any diff content
    ///     because it makes version very large when user auto click and don't change anything
    /// </summary>
    /// <param name="nameVersion"></param>
    /// <param name="diffTitle"></param>
    /// <param name="diffContent"></param>
    /// <param name="version"></param>
    /// <returns></returns>

    public static ChapterVersion? Create(string nameVersion, string diffTitle, string diffContent,
        uint version)
    {
        if (string.IsNullOrWhiteSpace(diffContent) && string.IsNullOrWhiteSpace(diffTitle))
        {
            return null;
        }
        return new ChapterVersion(nameVersion, diffContent, diffTitle, version);
    }

    public void RenameVersion(string nameVersion)
    {
        if (!string.IsNullOrWhiteSpace(NameVersion))
        {
            NameVersion = nameVersion;
        }
    }
}
