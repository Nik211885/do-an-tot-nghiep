namespace Application.BoundContext.UserProfileContext.Message;

public static class UserProfileValidationMessage
{
    public const string MaxBioLength = "Bio không thể lớn hơn 500 kí tự";
    public const string HasSyncUserFromKeyCloak = "Đã đồng bộ dữ liệu từ iam";
}
