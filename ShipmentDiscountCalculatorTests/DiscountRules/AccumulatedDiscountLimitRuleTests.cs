using ShipmentDiscountCalculator;
using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Enums;
using System;
using Xunit;

namespace ShipmentDiscountCalculatorTests.DiscountRules
{
    public class AccumulatedDiscountLimitRuleTests
    {
        [Fact]
        public void GetDiscount_WhenTransactionIsNull_ThrowsArgumentNullException()
        {
            var accumulatedDiscountLimitRule = new AccumulatedDiscountLimitRule(20);

            Assert.Throws<ArgumentNullException>("transaction", () => accumulatedDiscountLimitRule.GetDiscount(null, 10));
        }

        [Fact]
        public void GetDiscount_WhenAccumulatedDiscountIsLessThanLimit_ReturnsAccumulatedDiscount()
        {
            var limit = 20;
            var accumulatedDiscountLimitRule = new AccumulatedDiscountLimitRule(limit);
            var transaction = GetTransaction("2019-10-13");

            var discount = accumulatedDiscountLimitRule.GetDiscount(transaction, limit - 1);

            Assert.Equal(limit - 1, discount);
        }

        [Fact]
        public void GetDiscount_WhenAccumulatedDiscountIsEqualToLimit_ReturnsAccumulatedDiscount()
        {
            var limit = 20;
            var accumulatedDiscountLimitRule = new AccumulatedDiscountLimitRule(limit);
            var transaction = GetTransaction("2019-10-13");

            var discount = accumulatedDiscountLimitRule.GetDiscount(transaction, limit);

            Assert.Equal(limit, discount);
        }

        [Fact]
        public void GetDiscount_WhenTransactionsAreOnSameMonth_LimitsAccumulatedDiscount()
        {
            var limit = 20;
            var accumulatedDiscountLimitRule = new AccumulatedDiscountLimitRule(limit);
            var transaction1 = GetTransaction("2019-10-13");
            var transaction2 = GetTransaction("2019-10-14");
            var transaction3 = GetTransaction("2019-10-20");

            var discount1 = accumulatedDiscountLimitRule.GetDiscount(transaction1, 10);
            var discount2 = accumulatedDiscountLimitRule.GetDiscount(transaction2, 7);
            var discount3 = accumulatedDiscountLimitRule.GetDiscount(transaction3, 20);

            Assert.Equal(10, discount1);
            Assert.Equal(7, discount2);
            Assert.Equal(3, discount3);
        }

        [Fact]
        public void GetDiscount_WhenTransactionsAreOnDifferentMonths_LimitsByMonth()
        {
            var limit = 20;
            var accumulatedDiscountLimitRule = new AccumulatedDiscountLimitRule(limit);
            var firstMonthTransaction1 = GetTransaction("2019-10-13");
            var firstMonthTransaction2 = GetTransaction("2019-10-14");

            var secondMonthTransaction1 = GetTransaction("2019-11-10");
            var secondMonthTransaction2 = GetTransaction("2019-11-20");

            var discount1 = accumulatedDiscountLimitRule.GetDiscount(firstMonthTransaction1, 10);
            var discount2 = accumulatedDiscountLimitRule.GetDiscount(firstMonthTransaction2, 9);

            var discount3 = accumulatedDiscountLimitRule.GetDiscount(secondMonthTransaction1, 19);
            var discount4 = accumulatedDiscountLimitRule.GetDiscount(secondMonthTransaction2, 30);

            Assert.Equal(10, discount1);
            Assert.Equal(9, discount2);
            Assert.Equal(19, discount3);
            Assert.Equal(1, discount4);

        }

        private Transaction GetTransaction(string date) => new Transaction
        {
            Date = DateTime.Parse(date),
            Size = ShipmentSize.L,
            Type = ShipmentType.LP
        };
    }
}
