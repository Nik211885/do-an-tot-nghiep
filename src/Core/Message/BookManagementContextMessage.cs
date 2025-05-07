namespace Core.Message;

public static class BookManagementContextMessage
{
    public const string BookPaidPolicyButDontAddPrice 
        = "Bạn đang chọn chính sách trả phí, vui lòng nhập số tiền hợp lệ để hoàn thành chính sách viết sách";
    public const string BookNotPaidPolicyButAddPrice 
        = "Bạn đang không chọn chính sách trả phí, nên không thể đính kếm số tiền cho loại sách không trả phí";
    public const string ChapterHasLocked 
        = "Chương này đang bị khóa không thể cập nhật nội dung được";
    public const string CanNotPublishChapterBecauseChapterDontInDraftOrPeddingReview
        = "Chương này đang ở trạng thái {0} nên không thể xuất bản";
    public const string CanNotPublishChapterContentEmpty
        = "Nội dung chương sách này không được để trống khi xuất bản";

    public const string ChapterTitleCanNotNull
        = "Tiêu đề của chương sách không được để trống";

    public const string ChapterNumberCanNotBeImPositive
        = "Số chương phải là số dương";

    public const string CantApproveChapterNotInStatusIsSubmit
        = "Chỉ có thể duyệt chương đang được gửi hoặc đang kiểm duyệt.";
    public const string CantRejectChapterNotInStatusIsSubmit
        = "Chỉ có thể tu choi chương đang được gửi hoặc đang kiểm duyệt.";
}
