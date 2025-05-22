namespace Application.BoundContext.BookAuthoringContext.Message;

public static class GenreMessage
{
    public const string NameRequired = "Tên thể loại là bắt buộc.";
    public const string NameMaxLength = "Tên thể loại tối đa 100 ký tự.";
    public const string NameAlreadyExists = "Tên thể loại đã tồn tại.";

    public const string DescriptionRequired = "Mô tả là bắt buộc.";
    public const string DescriptionMaxLength = "Mô tả tối đa 500 ký tự.";

    public const string AvatarUrlRequired = "Ảnh đại diện là bắt buộc.";
    public const string AvatarUrlMaxLength = "Đường dẫn ảnh đại diện tối đa 200 ký tự.";
}
