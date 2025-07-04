namespace Application.BoundContext.UserProfileContext.Message;

public static class FollowerValidationMessage
{
    public const string CanNotFollowerYourSelf = "Bạn không thể theo dõi chính bạn";
    public const string YouHasFollowerUser = "Bạn đã theo dõi tác giả này rồi";
    public const string CanNotFindUserProfile = "không tìm thấy người đang theo dõi";
}
