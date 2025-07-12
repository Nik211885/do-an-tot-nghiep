namespace Application.BoundContext.OrderContext.ViewModel;

public class StatisticalPaymentViewModel(int coutBookBuy, decimal coutMoney)
{
    public int CoutBookBuy { get; } = coutBookBuy;
    public decimal CoutMoney { get; } =  coutMoney;
}

public class StatisticalBookPaymentViewModel(Guid bookId, int coutBuy, decimal coutMoney)
{
    public Guid BookId { get; } = bookId;
    public int CoutBuy { get; } =  coutBuy;
    public decimal CoutMoney { get; } =  coutMoney;
}
