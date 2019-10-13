using System.Collections.Generic;
using Moq;
using ShipmentDiscountCalculator;
using ShipmentDiscountCalculator.Enums;
using ShipmentDiscountCalculator.Services;
using Xunit;

namespace ShipmentDiscountCalculatorTests.Services
{
    public class TransactionPriceAppenderTests
    {
        private readonly TransactionPriceAppender _transactionPriceAppender;
        private readonly Mock<IDiscountCalculator> _discountCalculatorMock;

        public TransactionPriceAppenderTests()
        {
            _discountCalculatorMock = new Mock<IDiscountCalculator>();
            var prices = new Dictionary<(ShipmentType, ShipmentSize), double>
            {
                {(ShipmentType.LP, ShipmentSize.S), 1},
                {(ShipmentType.LP, ShipmentSize.M), 2},
                {(ShipmentType.LP, ShipmentSize.L), 3},
                {(ShipmentType.MR, ShipmentSize.S), 4},
                {(ShipmentType.MR, ShipmentSize.M), 5},
                {(ShipmentType.MR, ShipmentSize.L), 6}
            };

            _transactionPriceAppender = new TransactionPriceAppender(
                _discountCalculatorMock.Object,
                prices,
                "yyyy-MM-dd"
            );
        }

        [Theory]
        [InlineData("2019/10/13 S MR")]
        [InlineData("2019-10-13 S")]
        [InlineData("2019-10-13 MR")]
        [InlineData("2019-10-13 MR S")]
        [InlineData("2019-10-13 CUSPS")]
        [InlineData("")]
        public void Append_WhenLineIsInvalid_AppendsIgnoredFlag(string value)
        {
            var result = _transactionPriceAppender.Append(value);

            Assert.Equal($"{value} Ignored", result);
        }

        [Theory]
        [InlineData("2019-10-15 S LP", 1)]
        [InlineData("2019-10-13 M LP", 2)]
        [InlineData("2019-12-13 L LP", 3)]
        [InlineData("2019-10-13 S MR", 4)]
        [InlineData("2019-10-13 M MR", 5)]
        [InlineData("2019-10-14 L MR", 6)]
        public void Append_AppendsPriceWithoutDiscount_WhenThereIsNoDiscount(string value, double price)
        {
            _discountCalculatorMock
                .Setup(calculator => calculator.GetDiscount(It.IsAny<Transaction>()))
                .Returns(0);
            var result = _transactionPriceAppender.Append(value);

            Assert.Equal($"{value} {price:0.00} -", result);
        }
        
        [Theory]
        [InlineData("2019-10-15 S LP", 0.5, 0.5)]
        [InlineData("2019-10-13 M LP", 1.5, 0.5)]
        [InlineData("2019-12-13 L LP", 2, 1)]
        [InlineData("2019-10-13 S MR", 2, 2)]
        [InlineData("2019-10-13 M MR", 3, 2)]
        [InlineData("2019-10-14 L MR", 0, 6)]
        public void Append_AppendsPriceWithDiscount_WhenThereIsDiscount(string value, double finalPrice, double discount)
        {
            _discountCalculatorMock
                .Setup(calculator => calculator.GetDiscount(It.IsAny<Transaction>()))
                .Returns(discount);
            var result = _transactionPriceAppender.Append(value);

            Assert.Equal($"{value} {finalPrice:0.00} {discount:0.00}", result);
        }
    }
}
