using System;
using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;
using ShipmentDiscountCalculator.Entities;
using Xunit;

namespace ShipmentDiscountCalculatorTests.DiscountRules
{
    public class RepeatedSizeRuleTests
    {
        private const int RequiredRepetitionCount = 10;
        private readonly Dictionary<(ShipmentProvider, ShipmentSize), double> _prices =
            new Dictionary<(ShipmentProvider, ShipmentSize), double>
            {
                {(ShipmentProvider.LP, ShipmentSize.S), 1.50},
                {(ShipmentProvider.LP, ShipmentSize.M), 4.90},
                {(ShipmentProvider.LP, ShipmentSize.L), 6.90},
                {(ShipmentProvider.MR, ShipmentSize.S), 2},
                {(ShipmentProvider.MR, ShipmentSize.M), 3},
                {(ShipmentProvider.MR, ShipmentSize.L), 4}
            };

        private readonly DateTime _date = DateTime.Parse("2019-10-13");

        [Fact]
        public void GetDiscount_WhenTransactionIsNull_ThrowsArgumentNullException()
        {
            var rule = new RepeatedSizeRule(ShipmentSize.S, ShipmentProvider.MR, RequiredRepetitionCount, _prices);

            Assert.Throws<ArgumentNullException>("transaction", () => rule.GetDiscount(null, 10));
        }

        [Theory]
        [InlineData(ShipmentSize.S, ShipmentSize.M)]
        [InlineData(ShipmentSize.M, ShipmentSize.L)]
        [InlineData(ShipmentSize.L, ShipmentSize.S)]
        public void GetDiscount_WhenSizeDoesNotMatch_DoesNotApplyDiscount(ShipmentSize size, ShipmentSize requiredSize)
        {
            const ShipmentProvider provider = ShipmentProvider.MR;
            var rule = new RepeatedSizeRule(requiredSize, provider, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 0; i <= RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Provider = provider
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);
                
                Assert.Equal(currentDiscount, discount);
            }
        }
        [Theory]
        [InlineData(ShipmentProvider.LP, ShipmentProvider.MR)]
        [InlineData(ShipmentProvider.MR, ShipmentProvider.LP)]
        public void GetDiscount_WhenProviderDoesNotMatch_DoesNotApplyDiscount(ShipmentProvider provider, ShipmentProvider requiredProvider)
        {
            const ShipmentSize size = ShipmentSize.L;
            var rule = new RepeatedSizeRule(size, requiredProvider, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 0; i <= RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Provider = provider
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);

                Assert.Equal(currentDiscount, discount);
            }
        }

        [Theory]
        [InlineData(ShipmentProvider.LP, ShipmentSize.S)]
        [InlineData(ShipmentProvider.LP, ShipmentSize.M)]
        [InlineData(ShipmentProvider.LP, ShipmentSize.L)]
        [InlineData(ShipmentProvider.MR, ShipmentSize.S)]
        [InlineData(ShipmentProvider.MR, ShipmentSize.M)]
        [InlineData(ShipmentProvider.MR, ShipmentSize.L)]
        public void GetDiscount_WhenSizeAndProviderMatchAndSameMonth_ApplyDiscountForRequiredRepetitionCount(
            ShipmentProvider provider, ShipmentSize size)
        {
            var rule = new RepeatedSizeRule(size, provider, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 1; i < RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Provider = provider
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);

                Assert.Equal(currentDiscount, discount);
            }

            var finalTransaction = new Transaction
            {
                Date = _date,
                Size = size,
                Provider = provider
            };

            var discountForFreeTransaction = rule.GetDiscount(finalTransaction, currentDiscount);
            Assert.Equal(_prices[(provider, size)], discountForFreeTransaction);
        }

        [Theory]
        [InlineData(ShipmentProvider.LP, ShipmentSize.S)]
        [InlineData(ShipmentProvider.LP, ShipmentSize.M)]
        [InlineData(ShipmentProvider.LP, ShipmentSize.L)]
        [InlineData(ShipmentProvider.MR, ShipmentSize.S)]
        [InlineData(ShipmentProvider.MR, ShipmentSize.M)]
        [InlineData(ShipmentProvider.MR, ShipmentSize.L)]
        public void GetDiscount_WhenSizeAndProviderMatchButDifferentMonth_DoNotApplyDiscount(
            ShipmentProvider provider, ShipmentSize size)
        {
            var rule = new RepeatedSizeRule(size, provider, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 1; i < RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Provider = provider
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);

                Assert.Equal(currentDiscount, discount);
            }

            var finalTransaction = new Transaction
            {
                Date = _date.AddMonths(1),
                Size = size,
                Provider = provider
            };

            var discountForFreeTransaction = rule.GetDiscount(finalTransaction, currentDiscount);
            Assert.Equal(currentDiscount, discountForFreeTransaction);
        }
    }
}
