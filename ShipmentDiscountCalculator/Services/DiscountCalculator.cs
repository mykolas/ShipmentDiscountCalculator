using System.Collections.Generic;
using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Entities;

namespace ShipmentDiscountCalculator.Services
{
    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly IList<IDiscountRule> _rules;

        public DiscountCalculator(IList<IDiscountRule> rules)
        {
            _rules = rules;
        }

        public double GetDiscount(Transaction transaction)
        {
            var result = 0d;
            foreach (var rule in _rules)
            {
                result = rule.GetDiscount(transaction, result);
            }
            return result;
        }
    }
}
