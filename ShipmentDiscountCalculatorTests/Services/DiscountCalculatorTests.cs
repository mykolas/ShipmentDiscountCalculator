using System;
using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Services;
using Xunit;
using Moq;
using ShipmentDiscountCalculator;
using ShipmentDiscountCalculator.Enums;

namespace ShipmentDiscountCalculatorTests.Services
{
    public class DiscountCalculatorTests
    {
        private readonly Transaction _transaction = new Transaction
        {
            Date = DateTime.Parse("2019-10-13"),
            Size = ShipmentSize.L,
            Type = ShipmentType.LP
        };

        [Fact]
        public void GetDiscount_WhenThereAreNoRules_ReturnsZero()
        {
            var rules = Array.Empty<IDiscountRule>();
            var discountCalculator = new DiscountCalculator(rules);

            var discount = discountCalculator.GetDiscount(_transaction);

            Assert.Equal(0, discount);
        }

        [Fact]
        public void GetDiscount_AppliesAllRulesAndReturnsLastDiscount()
        {
            var rulesResults = new List<double> {1, 100, 0, 16, 17};
            var rules = rulesResults.Select(result =>
            {
                var mock = new Mock<IDiscountRule>();
                
                mock.Setup(rule =>
                        rule.GetDiscount(It.IsAny<Transaction>(), It.IsAny<double>()))
                    .Returns(result);

                return mock.Object;
            });

            var discountCalculator = new DiscountCalculator(rules.ToList());

            var discount = discountCalculator.GetDiscount(_transaction);

            Assert.Equal(rulesResults.Last(), discount);
        }
    }
}
