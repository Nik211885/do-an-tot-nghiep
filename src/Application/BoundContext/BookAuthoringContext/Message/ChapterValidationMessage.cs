using Application.BoundContext.BookAuthoringContext.Validator.Chapter;

namespace Application.BoundContext.BookAuthoringContext.Message;

public static class ChapterValidationMessage
{
    public static Dictionary<string, Dictionary<string, string>>
        NotFoundChapterById(Guid id) => new ()
        {
            ["Chương"] = new ()
            {
                ["Định danh"] = id.ToString()
            }
        };
    public const string ContentRequired = "Nội dung chương không được để trống";
    public const string TitleRequired = "Tiêu đề chương không được để trống";
    public static readonly string TitleMaxLenght = $"Tiêu đề của chương không được vượt quá {LengthPropForChapter.TitleMaxLength} kí tự";
    public const string NameVersionRequired = "Tên của phiên bản không được để trống";
    public static readonly string NameVersionMaxLenght = $"Tên của phiên không được vượt quá {LengthPropForChapter.MaxNameVersionChapter}";
}
