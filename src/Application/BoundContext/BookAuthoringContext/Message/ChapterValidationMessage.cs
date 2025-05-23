using Application.BoundContext.BookAuthoringContext.Validator.Chapter;

namespace Application.BoundContext.BookAuthoringContext.Message;

public static class ChapterValidationMessage
{
    public const string ContentRequired = "Nội dung chương không được để trống";
    public const string TitleRequired = "Tiêu đề chương không được để trống";
    public static readonly string TitleMaxLenght = $"Tiêu đề của chương không được vượt quá {LengthPropForChapter.TitleMaxLength} kí tự";
}
