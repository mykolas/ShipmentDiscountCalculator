using ShipmentDiscountCalculator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculator.Entities;

namespace ShipmentDiscountCalculator.DiscountRules
{
    /// <summary>
    /// Once a calendar month, every nth shipment of specified size and provider will be free
    /// </summary>
    public class RepeatedSizeRule : IDiscountRule
    {
        private readonly ShipmentSize _size;
        private readonly ShipmentProvider _provider;
        private readonly int _requiredRepetitionCount;
        private readonly IDictionary<ShipmentProvider, double> _priceByProvider;
        private int _currentRepetitionCount = 1;
        private DateTime _lastDate;

        public RepeatedSizeRule(ShipmentSize size, ShipmentProvider provider, int requiredRepetitionCount, IDictionary<(ShipmentProvider, ShipmentSize), double> prices)
        {
            _size = size;
            _provider = provider;
            _requiredRepetitionCount = requiredRepetitionCount;

            _priceByProvider = prices
                .Where(s => s.Key.Item2 == size)
                .ToDictionary(s => s.Key.Item1, s => s.Value);
        }

        public double GetDiscount(Transaction transaction, double currentDiscount)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction.Size != _size || transaction.Provider != _provider)
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
                return _priceByProvider[transaction.Provider];
            }

            return currentDiscount;
        }
    }
}
