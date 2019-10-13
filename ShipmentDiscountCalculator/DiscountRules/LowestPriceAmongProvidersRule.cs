using System;
using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculator.Entities;

namespace ShipmentDiscountCalculator.DiscountRules
{
    /// <summary>
    /// All shipments of specified size should always match the lowest package price for that size among the providers.
    /// </summary>
    public class LowestPriceAmongProvidersRule : IDiscountRule
    {
        private readonly ShipmentSize _size;
        private readonly IDictionary<ShipmentProvider, double> _priceByProvider;
        private readonly double _lowestPrice;

        public LowestPriceAmongProvidersRule(ShipmentSize size, IDictionary<(ShipmentProvider, ShipmentSize), double> prices)
        {
            _size = size;

            _priceByProvider = prices
                .Where(s => s.Key.Item2 == size)
                .ToDictionary(s => s.Key.Item1, s => s.Value);

            _lowestPrice = _priceByProvider.Min(s => s.Value);
        }

        public double GetDiscount(Transaction transaction, double currentDiscount)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction.Size != _size)
            {
                return currentDiscount;
            }

            var price = _priceByProvider[transaction.Provider];

            return price - _lowestPrice;
        }
    }
}
