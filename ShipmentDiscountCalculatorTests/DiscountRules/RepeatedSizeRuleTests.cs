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
        private readonly Dictionary<(ShipmentType, ShipmentSize), double> _prices =
            new Dictionary<(ShipmentType, ShipmentSize), double>
            {
                {(ShipmentType.LP, ShipmentSize.S), 1.50},
                {(ShipmentType.LP, ShipmentSize.M), 4.90},
                {(ShipmentType.LP, ShipmentSize.L), 6.90},
                {(ShipmentType.MR, ShipmentSize.S), 2},
                {(ShipmentType.MR, ShipmentSize.M), 3},
                {(ShipmentType.MR, ShipmentSize.L), 4}
            };

        private readonly DateTime _date = DateTime.Parse("2019-10-13");

        [Fact]
        public void GetDiscount_WhenTransactionIsNull_ThrowsArgumentNullException()
        {
            var rule = new RepeatedSizeRule(ShipmentSize.S, ShipmentType.MR, RequiredRepetitionCount, _prices);

            Assert.Throws<ArgumentNullException>("transaction", () => rule.GetDiscount(null, 10));
        }

        [Theory]
        [InlineData(ShipmentSize.S, ShipmentSize.M)]
        [InlineData(ShipmentSize.M, ShipmentSize.L)]
        [InlineData(ShipmentSize.L, ShipmentSize.S)]
        public void GetDiscount_WhenSizeDoesNotMatch_DoesNotApplyDiscount(ShipmentSize size, ShipmentSize requiredSize)
        {
            const ShipmentType type = ShipmentType.MR;
            var rule = new RepeatedSizeRule(requiredSize, type, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 0; i <= RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Type = type
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);
                
                Assert.Equal(currentDiscount, discount);
            }
        }
        [Theory]
        [InlineData(ShipmentType.LP, ShipmentType.MR)]
        [InlineData(ShipmentType.MR, ShipmentType.LP)]
        public void GetDiscount_WhenTypeDoesNotMatch_DoesNotApplyDiscount(ShipmentType type, ShipmentType requiredType)
        {
            const ShipmentSize size = ShipmentSize.L;
            var rule = new RepeatedSizeRule(size, requiredType, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 0; i <= RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Type = type
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);

                Assert.Equal(currentDiscount, discount);
            }
        }

        [Theory]
        [InlineData(ShipmentType.LP, ShipmentSize.S)]
        [InlineData(ShipmentType.LP, ShipmentSize.M)]
        [InlineData(ShipmentType.LP, ShipmentSize.L)]
        [InlineData(ShipmentType.MR, ShipmentSize.S)]
        [InlineData(ShipmentType.MR, ShipmentSize.M)]
        [InlineData(ShipmentType.MR, ShipmentSize.L)]
        public void GetDiscount_WhenSizeAndTypeMatchAndSameMonth_ApplyDiscountForRequiredRepetitionCount(ShipmentType type, ShipmentSize size)
        {
            var rule = new RepeatedSizeRule(size, type, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 1; i < RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Type = type
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);

                Assert.Equal(currentDiscount, discount);
            }

            var finalTransaction = new Transaction
            {
                Date = _date,
                Size = size,
                Type = type
            };

            var discountForFreeTransaction = rule.GetDiscount(finalTransaction, currentDiscount);
            Assert.Equal(_prices[(type, size)], discountForFreeTransaction);
        }

        [Theory]
        [InlineData(ShipmentType.LP, ShipmentSize.S)]
        [InlineData(ShipmentType.LP, ShipmentSize.M)]
        [InlineData(ShipmentType.LP, ShipmentSize.L)]
        [InlineData(ShipmentType.MR, ShipmentSize.S)]
        [InlineData(ShipmentType.MR, ShipmentSize.M)]
        [InlineData(ShipmentType.MR, ShipmentSize.L)]
        public void GetDiscount_WhenSizeAndTypeMatchButDifferentMonth_DoNotApplyDiscount(ShipmentType type, ShipmentSize size)
        {
            var rule = new RepeatedSizeRule(size, type, RequiredRepetitionCount, _prices);
            const int currentDiscount = 100;

            for (var i = 1; i < RequiredRepetitionCount; i++)
            {
                var transaction = new Transaction
                {
                    Date = _date,
                    Size = size,
                    Type = type
                };
                var discount = rule.GetDiscount(transaction, currentDiscount);

                Assert.Equal(currentDiscount, discount);
            }

            var finalTransaction = new Transaction
            {
                Date = _date.AddMonths(1),
                Size = size,
                Type = type
            };

            var discountForFreeTransaction = rule.GetDiscount(finalTransaction, currentDiscount);
            Assert.Equal(currentDiscount, discountForFreeTransaction);
        }
    }
}
