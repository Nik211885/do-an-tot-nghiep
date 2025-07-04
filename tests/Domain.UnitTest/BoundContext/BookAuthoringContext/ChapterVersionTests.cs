using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Domain.UnitTest.BoundContext.BookAuthoringContext;

public class ChapterVersionTests
{
    [Fact]
    public void Create_Should_Return_Instance_When_Valid()
    {
        var version = ChapterVersion.Create("v1", "diffTitle", "diffContent", 1);
        Assert.NotNull(version);
        Assert.Equal("v1", version.NameVersion);
        Assert.Equal("diffTitle", version.DiffTitle);
        Assert.Equal("diffContent", version.DiffContent);
        Assert.Equal((uint)1, version.Version);
    }

    [Fact]
    public void Create_Should_Return_Null_When_Diff_Empty()
    {
        var version = ChapterVersion.Create("v1", "", "", 1);
        Assert.Null(version);
    }

    [Fact]
    public void RenameVersion_Should_Change_Name()
    {
        var version = ChapterVersion.Create("v1", "diffTitle", "diffContent", 1);
        version.RenameVersion("newName");
        Assert.Equal("newName", version.NameVersion);
    }
}
