using System;
using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculator.Entities;
using Xunit;

namespace ShipmentDiscountCalculatorTests.DiscountRules
{
    public class LowestPriceAmongProvidersRuleTests
    {
        private readonly Dictionary<(ShipmentProvider, ShipmentSize), double> _prices;
        private readonly DateTime _date = DateTime.Parse("2019-10-13");
        private readonly Dictionary<ShipmentSize, double> _lowestPriceBySize;

        public LowestPriceAmongProvidersRuleTests()
        {
            _prices = new Dictionary<(ShipmentProvider, ShipmentSize), double>
            {
                {(ShipmentProvider.LP, ShipmentSize.S), 1.50 },
                {(ShipmentProvider.LP, ShipmentSize.M), 4.90 },
                {(ShipmentProvider.LP, ShipmentSize.L), 6.90 },
                {(ShipmentProvider.MR, ShipmentSize.S), 2 },
                {(ShipmentProvider.MR, ShipmentSize.M), 3 },
                {(ShipmentProvider.MR, ShipmentSize.L), 4 }
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
        [InlineData(ShipmentSize.L, ShipmentProvider.LP, ShipmentSize.S)]
        [InlineData(ShipmentSize.M, ShipmentProvider.LP, ShipmentSize.S)]
        [InlineData(ShipmentSize.L, ShipmentProvider.MR, ShipmentSize.S)]
        [InlineData(ShipmentSize.M, ShipmentProvider.MR, ShipmentSize.S)]
        public void GetDiscount_WhenSizeDoesNotMatch_DoesNotChangeDiscount(ShipmentSize size, ShipmentProvider provider, ShipmentSize requiredSize)
        {
            var rule = new LowestPriceAmongProvidersRule(requiredSize, _prices);
            var transaction = new Transaction { Date = _date, Size = size, Provider = provider };
            const int currentDiscount = 10;

            var newDiscount = rule.GetDiscount(transaction, currentDiscount);

            Assert.Equal(currentDiscount, newDiscount);
        }

        [Theory]
        [InlineData(ShipmentSize.S, ShipmentProvider.LP)]
        [InlineData(ShipmentSize.S, ShipmentProvider.MR)]
        [InlineData(ShipmentSize.M, ShipmentProvider.LP)]
        [InlineData(ShipmentSize.M, ShipmentProvider.MR)]
        [InlineData(ShipmentSize.L, ShipmentProvider.LP)]
        [InlineData(ShipmentSize.L, ShipmentProvider.MR)]
        public void GetDiscount_WhenSizeMatch_ReturnsLowestSizePriceAmongProviders(ShipmentSize requiredSize, ShipmentProvider provider)
        {
            var rule = new LowestPriceAmongProvidersRule(requiredSize, _prices);
            var transaction = new Transaction { Date = _date, Size = requiredSize, Provider = provider };
            const int currentDiscount = 10;

            var newDiscount = rule.GetDiscount(transaction, currentDiscount);

            var expectedDiscount = _prices[(provider, requiredSize)] - _lowestPriceBySize[requiredSize];
            Assert.Equal(expectedDiscount, newDiscount);
        }
    }
}
