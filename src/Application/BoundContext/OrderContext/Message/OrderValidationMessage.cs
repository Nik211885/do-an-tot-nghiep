namespace Application.BoundContext.OrderContext.Message;

public static class OrderValidationMessage
{
    public const string ReturnUrlInvalid = "Địa chỉ ứng dụng không hợp lệ";
    public const string BookDontHasPaid = "Sách không phải sách trả phí không thể mua";
    public const string DontHaveBookInOrder = "Không có sách trong đơn của bạn";
}
