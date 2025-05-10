using Core.Entities;
using Core.Events.AuthoringContext;
using Core.Exception;
using Core.Message;

namespace Core.BoundContext.WriteBookContext.ChapterAggregate;

public class ChapterVersion : BaseEntity
{
    private Guid _chapterId;
    public Chapter Chapter { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public string DiffContent { get; private set; }
    public uint Version { get; private set; }
    public string DiffTitle { get; private set; }

    /*public string ContentSnapShot { get; private set; }*/
    /*public string TitleSnapShot { get;private set; }*/
    //public bool IsSnapShot { get; private set; }
    /*public bool IsRollBack { get; private set; }*/

    private ChapterVersion(Guid chapterId, string diffContent, string diffTitle, uint version)
    {
        _chapterId = chapterId;
        DiffTitle = diffTitle;
        DiffContent = diffContent;
        Version = version;
        CreatedDateTime = DateTimeOffset.UtcNow;
    }
    
    public static ChapterVersion Create(Guid chapterId, string diffTitle, string diffContent, uint version)
    {
        return new ChapterVersion(chapterId, diffTitle, diffContent, version);
    }
}
