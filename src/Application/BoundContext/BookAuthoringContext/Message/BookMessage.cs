namespace Application.BoundContext.BookAuthoringContext.Message;

public static class BookMessage
{
    public const string TitleRequired = "Tiêu đề sách không được để trống.";
    public const string TitleMaxLength50 = "Tiêu đề sách không được vượt quá 50 ký tự.";
    public const string AvatarUrlMaxLength250 = "Đường dẫn ảnh đại diện không được vượt quá 250 ký tự.";
    public const string DescriptionMaxLength500 = "Mô tả sách không được vượt quá 500 ký tự.";
    public const string ReaderBookPolicyInvalid = "Chính sách đọc sách không hợp lệ.";
    public const string BookReleaseTypeInvalid = "Loại phát hành sách không hợp lệ.";
    public const string TagNameRequired = "Tên tag không được để trống.";
    public const string TagNameMaxLength50 = "Tên tag không được vượt quá 50 ký tự.";
    public const string GenreIdsRequired = "Phải chọn ít nhất một thể loại.";
    public const string VersionNumberGreaterThanZero = "Số phiên bản phải lớn hơn 0.";
}
