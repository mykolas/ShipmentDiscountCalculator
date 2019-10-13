using System;
using Xunit;
using ShipmentDiscountCalculator.DiscountRules;

namespace ShipmentDiscountCalculatorTests.DiscountRules
{
    public class AccumulatedDiscountLimitRuleTests
    {
        public AccumulatedDiscountLimitRuleTests()
        {
            var maxDiscount = 1;
            AccumulatedDiscountLimitRule _accumulatedDiscountLimitRule = new AccumulatedDiscountLimitRule(maxDiscount);
        }

        public void GetDiscount_WhenTransactionIsNull_ReturnsZero()
        {

        }

        public void GetDiscount_WhenAccumulatedDiscountIsLessThanLimit_ReturnsAccumulatedDiscount()
        {

        }

        public void GetDiscount_WhenTransactionsAreOnSameMonth_LimitsAccumulatedDiscount()
        {

        }

        public void GetDiscount_WhenTransactionsAreOnDifferentMonths_LimitsByMonth()
        {
            
        }
    }
}
