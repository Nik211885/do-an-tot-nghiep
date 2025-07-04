namespace Core.Message;

public static class BookReviewContextMessage
{
    public const string RatingStarNotFormat
        = "Đánh giá chỉ nằm trong 1-5 sao";

    public const string CanNotFindCommentId
        = "Không thể tìm thấy đánh giá";

    public const string JustHaveOneRatingInTheBook
        = "Bạn chỉ có thể đánh giá tác phẩm 1 lần ";

    public const string CanNotFindYourRating
        = "Không tìm thấy đánh giá của bạn";
}
