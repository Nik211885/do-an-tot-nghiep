using Core.Entities;

namespace Core.BoundContext.BookAuthoringContext.ChapterAggregate;

public class ChapterVersion : BaseEntity
{
    private Guid _chapterId;
    public Chapter Chapter { get; private set; }
    /// <summary>
    /// It make use rembember what name is history
    /// </summary>
    public string? NameVersion { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public string DiffContent { get; private set; }
    public uint Version { get; private set; }
    public string DiffTitle { get; private set; }

    /*public string ContentSnapShot { get; private set; }*/
    /*public string TitleSnapShot { get;private set; }*/
    //public bool IsSnapShot { get; private set; }
    /*public bool IsRollBack { get; private set; }*/

    private ChapterVersion(Guid chapterId, string nameVersion, string diffContent, string diffTitle, uint version)
    {
        NameVersion = nameVersion;
        _chapterId = chapterId;
        DiffTitle = diffTitle;
        DiffContent = diffContent;
        Version = version;
        CreatedDateTime = DateTimeOffset.UtcNow;
    }

    public static ChapterVersion Create(Guid chapterId, string nameVersion, string diffTitle, string diffContent,
        uint version)
    {
        return new ChapterVersion(chapterId, nameVersion, diffTitle, diffContent, version);
    }

    public void UpdateNameVersion(string newNameVersion)
    {
        NameVersion = newNameVersion;
    }

}
