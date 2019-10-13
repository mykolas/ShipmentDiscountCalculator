using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;

namespace ShipmentDiscountCalculatorTests.DiscountRules
{
    public class RepeatedSizeRuleTests
    {
        private readonly Dictionary<(ShipmentType, ShipmentSize), double> _prices;
        private readonly int _requiredRepetitionCount;
        private readonly LowestPriceAmongProvidersRule _accumulatedDiscountLimitRule;
        private readonly RepeatedSizeRule _repeatedSizeRule;

        public RepeatedSizeRuleTests()
        {
            _requiredRepetitionCount = 10;
            _prices = new Dictionary<(ShipmentType, ShipmentSize), double>();
            _repeatedSizeRule = new RepeatedSizeRule(ShipmentSize.S, ShipmentType.MR, _requiredRepetitionCount, _prices);
        }

        public void GetDiscount_WhenTransactionIsNull_ReturnsZero()
        {

        }

        public void GetDiscount_WhenSizeDoesNotMatch_DoesNotApplyDiscount()
        {

        }

        public void GetDiscount_WhenTypeDoesNotMatch_DoesNotApplyDiscount()
        {

        }

        public void GetDiscount_WhenSizeTypeMatch_ApplyDiscountForRequiredRepetitionCount(int repetitionCount, bool shouldApplyDiscount)
        {

        }
    }
}
