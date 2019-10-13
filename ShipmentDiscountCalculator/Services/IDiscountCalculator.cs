using ShipmentDiscountCalculator.Entities;

namespace ShipmentDiscountCalculator.Services
{
    public interface IDiscountCalculator
    {
        double GetDiscount(Transaction transaction);
    }
}
