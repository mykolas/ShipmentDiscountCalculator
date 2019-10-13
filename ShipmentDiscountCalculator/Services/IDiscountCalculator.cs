namespace ShipmentDiscountCalculator
{
    public interface IDiscountCalculator
    {
        double GetDiscount(Transaction transaction);
    }
}
