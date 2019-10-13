using System;
using ShipmentDiscountCalculator.Entities;

namespace ShipmentDiscountCalculator.DiscountRules
{
    /// <summary>
    ///  Accumulated discounts cannot exceed provided limit in a calendar month. If there are not enough funds to fully
    /// cover a discount this calendar month, it should be covered partially.
    /// </summary>
    public class AccumulatedDiscountLimitRule : IDiscountRule
    {
        private DateTime _lastDate;
        private double _remainingDiscount;
        private readonly double _maxDiscount;

        public AccumulatedDiscountLimitRule(double maxDiscount)
        {
            _remainingDiscount = maxDiscount;
            _maxDiscount = maxDiscount;
        }

        public double GetDiscount(Transaction transaction, double currentDiscount)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (_lastDate.Month != transaction.Date.Month || _lastDate.Year != transaction.Date.Year)
            {
                _remainingDiscount = _maxDiscount;
            }

            _lastDate = transaction.Date;

            var discount = Math.Min(_remainingDiscount, currentDiscount);

            _remainingDiscount = Math.Max(0, _remainingDiscount - currentDiscount);

            return discount;
        }
    }
}
