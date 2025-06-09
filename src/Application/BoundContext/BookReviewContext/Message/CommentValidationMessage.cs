namespace Application.BoundContext.BookReviewContext.Message;

internal class CommentValidationMessage
{
    public const string ContentMaxLength = "Bình luận không thể dài quá 200 kí tự";
    public const string ContentCanNotNull = "Bình luận không thể để trống";
}
