namespace ShipmentDiscountCalculator.DiscountRules
{
    public interface IDiscountRule
    {
        double GetDiscount(Transaction transaction, double currentDiscount);
    }
}
