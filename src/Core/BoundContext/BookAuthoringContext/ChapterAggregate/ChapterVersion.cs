using Core.Entities;

namespace Core.BoundContext.BookAuthoringContext.ChapterAggregate;

public class ChapterVersion : BaseEntity
{
    /// <summary>
    /// It make use rembember what name is history
    /// </summary>
    ///
    // Shadow prop id chapter
    public string? NameVersion { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public string DiffContent { get; private set; }
    public uint Version { get; private set; }
    public string DiffTitle { get; private set; }

    /*public string ContentSnapShot { get; private set; }*/
    /*public string TitleSnapShot { get;private set; }*/
    //public bool IsSnapShot { get; private set; }
    /*public bool IsRollBack { get; private set; }*/
    protected ChapterVersion(){}
    private ChapterVersion(string nameVersion, string diffContent, string diffTitle, uint version)
    {
        NameVersion = nameVersion;
        DiffTitle = diffTitle;
        DiffContent = diffContent;
        Version = version;
        CreatedDateTime = DateTimeOffset.UtcNow;
    }

    public static ChapterVersion Create(string nameVersion, string diffTitle, string diffContent,
        uint version)
    {
        return new ChapterVersion(nameVersion, diffTitle, diffContent, version);
    }
    
    public void UpdateNameVersion(string newNameVersion)
    {
        NameVersion = newNameVersion;
    }

}
