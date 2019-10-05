using System;
using System.Collections.Generic;
using System.Text;

namespace ShipmentDiscountCalculator.DiscountRules
{
    public interface IDiscountRule
    {
        double GetDiscount(Transaction transaction);
    }

    // All S shipments should always match the lowest S package price among the providers.
    public class LowestPriceAmongProvidersRule : IDiscountRule
    {
        public double GetDiscount(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }

    // Third L shipment via LP should be free, but only once a calendar month.
    public class RepetedSizeRule : IDiscountRule
    {
        public double GetDiscount(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }

    public class AccumulatedDiscountLimitRule : IDiscountRule
    {
        public double GetDiscount(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
