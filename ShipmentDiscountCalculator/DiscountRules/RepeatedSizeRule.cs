using ShipmentDiscountCalculator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculator.Entities;

namespace ShipmentDiscountCalculator.DiscountRules
{
    public class RepeatedSizeRule : IDiscountRule
    {
        private readonly ShipmentSize _size;
        private readonly ShipmentType _type;
        private readonly int _requiredRepetitionCount;
        private readonly IDictionary<ShipmentType, double> _priceByType;
        private int _currentRepetitionCount = 1;
        private DateTime _lastDate;

        public RepeatedSizeRule(ShipmentSize size, ShipmentType type, int requiredRepetitionCount, IDictionary<(ShipmentType, ShipmentSize), double> prices)
        {
            _size = size;
            _type = type;
            _requiredRepetitionCount = requiredRepetitionCount;

            _priceByType = prices
                .Where(s => s.Key.Item2 == size)
                .ToDictionary(s => s.Key.Item1, s => s.Value);
        }

        public double GetDiscount(Transaction transaction, double currentDiscount)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction.Size != _size || transaction.Type != _type)
            {
                return currentDiscount;
            }

            if (_lastDate.Month != transaction.Date.Month || _lastDate.Year != transaction.Date.Year)
            {
                _currentRepetitionCount = 1;
            }

            _lastDate = transaction.Date;

            if (_currentRepetitionCount++ == _requiredRepetitionCount)
            {
                return _priceByType[transaction.Type];
            }

            return currentDiscount;
        }
    }
}
