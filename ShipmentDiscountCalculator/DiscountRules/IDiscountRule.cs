using System.Collections.Generic;
using System.Text;

namespace ShipmentDiscountCalculator.DiscountRules
{
    public interface IDiscountRule
    {
        double GetDiscount(Transaction transaction, double accumulatedDiscount);
    }
}
