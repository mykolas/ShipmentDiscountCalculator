using System;
using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculator;
using Xunit;

namespace ShipmentDiscountCalculatorTests.DiscountRules
{
    public class LowestPriceAmongProvidersRuleTests
    {
        private readonly Dictionary<(ShipmentType, ShipmentSize), double> _prices;
        private readonly DateTime _date = DateTime.Parse("2019-10-13");
        private readonly Dictionary<ShipmentSize, double> _lowestPriceBySize;

        public LowestPriceAmongProvidersRuleTests()
        {
            _prices = new Dictionary<(ShipmentType, ShipmentSize), double>()
            {
                {(ShipmentType.LP, ShipmentSize.S), 1.50 },
                {(ShipmentType.LP, ShipmentSize.M), 4.90 },
                {(ShipmentType.LP, ShipmentSize.L), 6.90 },
                {(ShipmentType.MR, ShipmentSize.S), 2 },
                {(ShipmentType.MR, ShipmentSize.M), 3 },
                {(ShipmentType.MR, ShipmentSize.L), 4 }
            };

            _lowestPriceBySize = _prices
                .GroupBy(price => price.Key.Item2)
                .ToDictionary(
                    group => group.Key,
                    group => group.Min(price => price.Value));
        }

        [Fact]
        public void GetDiscount_WhenTransactionIsNull_ThrowsArgumentNullException()
        {
            var rule = new LowestPriceAmongProvidersRule(ShipmentSize.S, _prices);

            Assert.Throws<ArgumentNullException>("transaction", () => rule.GetDiscount(null, 10));
        }

        [Theory]
        [InlineData(ShipmentSize.L, ShipmentType.LP, ShipmentSize.S)]
        [InlineData(ShipmentSize.M, ShipmentType.LP, ShipmentSize.S)]
        [InlineData(ShipmentSize.L, ShipmentType.MR, ShipmentSize.S)]
        [InlineData(ShipmentSize.M, ShipmentType.MR, ShipmentSize.S)]
        public void GetDiscount_WhenSizeDoesNotMatch_DoesNotChangeDiscount(ShipmentSize size, ShipmentType type, ShipmentSize requiredSize)
        {
            var rule = new LowestPriceAmongProvidersRule(requiredSize, _prices);
            var transaction = new Transaction { Date = _date, Size = size, Type = type };
            const int currentDiscount = 10;

            var newDiscount = rule.GetDiscount(transaction, currentDiscount);

            Assert.Equal(currentDiscount, newDiscount);
        }

        [Theory]
        [InlineData(ShipmentSize.S, ShipmentType.LP)]
        [InlineData(ShipmentSize.S, ShipmentType.MR)]
        [InlineData(ShipmentSize.M, ShipmentType.LP)]
        [InlineData(ShipmentSize.M, ShipmentType.MR)]
        [InlineData(ShipmentSize.L, ShipmentType.LP)]
        [InlineData(ShipmentSize.L, ShipmentType.MR)]
        public void GetDiscount_WhenSizeMatch_ReturnsLowestSizePriceAmongProviders(ShipmentSize requiredSize, ShipmentType type)
        {
            var rule = new LowestPriceAmongProvidersRule(requiredSize, _prices);
            var transaction = new Transaction { Date = _date, Size = requiredSize, Type = type };
            const int currentDiscount = 10;

            var newDiscount = rule.GetDiscount(transaction, currentDiscount);

            var expectedDiscount = _prices[(type, requiredSize)] - _lowestPriceBySize[requiredSize];
            Assert.Equal(expectedDiscount, newDiscount);
        }
    }
}
