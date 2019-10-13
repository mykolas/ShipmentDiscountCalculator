using System;
using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculator.Entities;

namespace ShipmentDiscountCalculator.DiscountRules
{
    public class LowestPriceAmongProvidersRule : IDiscountRule
    {
        private readonly ShipmentSize _size;
        private readonly IDictionary<ShipmentType, double> _priceByType;
        private readonly double _lowestPrice;

        public LowestPriceAmongProvidersRule(ShipmentSize size, IDictionary<(ShipmentType, ShipmentSize), double> prices)
        {
            _size = size;

            _priceByType = prices
                .Where(s => s.Key.Item2 == size)
                .ToDictionary(s => s.Key.Item1, s => s.Value);

            _lowestPrice = _priceByType.Min(s => s.Value);
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

            var price = _priceByType[transaction.Type];

            return price - _lowestPrice;
        }
    }
}
