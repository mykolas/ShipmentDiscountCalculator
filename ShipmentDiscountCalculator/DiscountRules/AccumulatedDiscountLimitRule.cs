using System;

namespace ShipmentDiscountCalculator.DiscountRules
{
    public class AccumulatedDiscountLimitRule : IDiscountRule
    {
        private DateTime lastDate;
        private double _remainingDiscount;
        private readonly double _maxDiscount;

        public AccumulatedDiscountLimitRule(double maxDiscount)
        {
            _remainingDiscount = maxDiscount;
            _maxDiscount = maxDiscount;
        }

        public double GetDiscount(Transaction transaction, double accumulatedDiscount)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (lastDate.Month != transaction.Date.Month || lastDate.Year != transaction.Date.Year)
            {
                _remainingDiscount = _maxDiscount;
            }

            lastDate = transaction.Date;

            var discount = Math.Min(_remainingDiscount, accumulatedDiscount);

            _remainingDiscount = Math.Max(0, _remainingDiscount - accumulatedDiscount);

            return discount;
        }
    }
}
