using Application.BoundContext.BookAuthoringContext.Validator.Book;

namespace Application.BoundContext.BookAuthoringContext.Message;

public static class BookValidationMessages
{
    public static Dictionary<string, Dictionary<string, string>>
        NoFoundBookById(Guid id) =>
        new ()
        {
            ["Sách"]= new ()
            {
                ["Định danh"] = id.ToString()
            }
        };
    public const string TitleRequired = "Tiêu đề sách không được để trống.";
    public static readonly string TitleMaxLength = $"Tiêu đề sách không được vượt quá {LengthPropForBook.TitleMaxLenght} ký tự.";
    public static readonly string AvatarUrlMaxLength = $"Đường dẫn ảnh đại diện không được vượt quá {LengthPropForBook.AvatarUrlMaxLenght} ký tự.";
    public static readonly string DescriptionCanNotNull = $"Mô tả sách không được để trống";
    public const string ReaderBookPolicyInvalid = "Chính sách đọc sách không hợp lệ.";
    public const string BookReleaseTypeInvalid = "Loại phát hành sách không hợp lệ.";
    public const string TagNameRequired = "Tên tag không được để trống.";
    public static readonly string TagNameMaxLength = $"Tên tag không được vượt quá {LengthPropForBook.TagNameMaxLenght} ký tự.";
    public const string GenreIdsRequired = "Phải chọn ít nhất một thể loại.";
    public const string VersionNumberGreaterThanZero = "Số phiên bản phải lớn hơn 0.";
    public const string HasGenreNotExit = "Có thể loại đã không còn tồn tại, vui lòng quay lại và chọn lại thể loại.";
}
