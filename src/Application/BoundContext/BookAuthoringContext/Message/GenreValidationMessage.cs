using Application.BoundContext.BookAuthoringContext.Validator.Genre;

namespace Application.BoundContext.BookAuthoringContext.Message;

public static class GenreValidationMessage
{
    public static Dictionary<string, Dictionary<string, string>>
        NoFoundGenreById(Guid id) =>
        new ()
        {
            ["Thể loại"]= new ()
            {
                ["Định danh"] = id.ToString()
            }
        };
    public const string NameRequired = "Tên thể loại là bắt buộc.";
    public static readonly string NameMaxLength = $"Tên thể loại tối đa {LengthPropGenre.NameMaxLenght} ký tự.";
    public const string NameAlreadyExists = "Tên thể loại đã tồn tại.";

    public const string DescriptionRequired = "Mô tả là bắt buộc.";
    public static readonly string DescriptionMaxLength = $"Mô tả tối đa {LengthPropGenre.DescriptionMaxLenght} ký tự.";

    public const string AvatarUrlRequired = "Ảnh đại diện là bắt buộc.";
    public static readonly string AvatarUrlMaxLength = $"Đường dẫn ảnh đại diện tối đa {LengthPropGenre.AvatarUrlMaxLenght} ký tự.";
}
