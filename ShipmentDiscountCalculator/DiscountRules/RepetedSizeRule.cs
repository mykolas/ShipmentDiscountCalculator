using ShipmentDiscountCalculator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentDiscountCalculator.DiscountRules
{
    public class RepeatedSizeRule : IDiscountRule
    {
        private readonly ShipmentSize _size;
        private readonly ShipmentType _type;
        private readonly int _requiredRepetitionCount;
        private readonly IDictionary<ShipmentType, double> _priceByType;
        private int _currentRepetitionCount = 1;
        private DateTime lastDate;

        public RepeatedSizeRule(ShipmentSize size, ShipmentType type, int requiredRepetitionCount, IDictionary<(ShipmentType, ShipmentSize), double> prices)
        {
            _size = size;
            _type = type;
            _requiredRepetitionCount = requiredRepetitionCount;

            _priceByType = prices
                .Where(s => s.Key.Item2 == size)
                .ToDictionary(s => s.Key.Item1, s => s.Value);
        }

        public double GetDiscount(Transaction transaction, double accumulatedDiscount)
        {
            if (transaction.Size != _size || transaction.Type != _type)
            {
                return accumulatedDiscount;
            }

            if (lastDate.Month != transaction.Date.Month || lastDate.Year != transaction.Date.Year)
            {
                _currentRepetitionCount = 1;
            }

            lastDate = transaction.Date;

            if (_currentRepetitionCount++ == _requiredRepetitionCount)
            {
                return _priceByType[transaction.Type];
            }

            return accumulatedDiscount;
        }
    }
}
