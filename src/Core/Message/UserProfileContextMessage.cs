namespace Core.Message;

public static class UserProfileContextMessage
{
    public const string YouHasFollowedUser =
        "Bạn đã theo dõi người dùng này rồi";

    public const string YouDontHasFollowedUser =
        "Bạn chưa theo dõi người dùng này";

    public const string YouHasFavoriteItem =
        "Bạn đã chọn yêu thích mục này rồi";

    public const string YouDontHaveFavoriteItem =
        "Bạn chưa chọn yêu thích mục này";

    public const string YouCanNotFollowYourself =
        "Bạn không thể tự theo dõi chính mình";
}
