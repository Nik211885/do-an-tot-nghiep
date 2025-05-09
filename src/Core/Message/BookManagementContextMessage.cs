namespace Core.Message;

public static class BookManagementContextMessage
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
}
