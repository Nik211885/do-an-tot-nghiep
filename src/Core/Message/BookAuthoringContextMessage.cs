namespace Core.Message;

public static class BookAuthoringContextMessage
{
    public const string BookPaidPolicyButDontAddPrice 
        = "Bạn đang chọn chính sách trả phí, vui lòng nhập số tiền hợp lệ để hoàn thành chính sách viết sách";
    public const string BookNotPaidPolicyButAddPrice 
        = "Bạn đang không chọn chính sách trả phí, nên không thể đính kếm số tiền cho loại sách không trả phí";
    
    public const string TagBookHasExits 
        = "Đã có tag này cho tác phẩm này rồi";

    public const string CanNotRemoveGenreIsEmpty
        = "Thể loại bắt buộc phải có ít nhất 1 thể loại";

    public const string CanNotRollBackChapterVersionCurrentVersion
        = "Không thể quay lại phiên hiện tại hoặc không tồn tại";
    
    public const string ChapterHasLocked
        = "Chương này đang gửi xuất bản nên không thể sửa";

    public const string ChapterFindBackVersion
        = "Không  tìm thấy sư thay đổi phù hợp";

    public const string CanNotFindChapterHasId
        = "Không tìm thấy chương có id là {0}";

    public const string CanNotExitsGenresInYourBook
        = "Thể loại bạn muốn xóa khỏi tác phẩm, không có trong tác phẩm của bạn";

    public const string BookCanNotDuplicateGenre
        = "Tác phẩm của bạn đã có thể loại này rồi!";

    public const string DuplicateBookGenre
        = "Sách của bạn không thể có hai thể loại giống nhau!";

    public const string DuplicateBookTags
        = "Sách của bạn không thể có hai thẻ có tên giống nhau!";

    public const string YourBookMustHasMoreThanOneGenre
        = "Sách của bạn phải chọn ít nhất một thể loại";

    public const string TagNotExitsInBook =
        "Thẻ này không có trong tác phẩm của bạn";

    public const string YouCanNotRollBackInCurrentVersion
        = "Bạn đang ở phiên bản này rồi không cần khôi phục!";

    public const string CanNoFindYourChapterVersion
        = "Không tìm thấy phiên bản mà bạn đã chọn!";

    public const string YouHasSubmitChapter
        = "Bạn đã gửi rồi vùi lòng chờ ít thời gian để chúng tôi kiểm tr tác phẩm của bạn";
}
