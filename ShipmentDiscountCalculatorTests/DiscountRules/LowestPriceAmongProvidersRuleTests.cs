using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;

namespace ShipmentDiscountCalculatorTests.DiscountRules
{
    public class LowestPriceAmongProvidersRuleTests
    {
        private readonly Dictionary<(ShipmentType, ShipmentSize), double> _prices;
        private readonly LowestPriceAmongProvidersRule _accumulatedDiscountLimitRule;

        public LowestPriceAmongProvidersRuleTests()
        {
            _prices = new Dictionary<(ShipmentType, ShipmentSize), double>();
            _accumulatedDiscountLimitRule = new LowestPriceAmongProvidersRule(ShipmentSize.S, _prices);
        }

        public void GetDiscount_WhenTransactionIsNull_ReturnsZero()
        {

        }

        public void GetDiscount_WhenSizeDoesNotMatch_ReturnsDefaultPrice()
        {

        }

        public void GetDiscount_WhenSizeMatch_ReturnsLowestSizePriceAmongProviders()
        {

        }
    }
}
