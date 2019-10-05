namespace ShipmentDiscountCalculator
{
    public interface IDiscountService
    {
        double GetTransactionDiscount(Transaction transaction);
    }
}
