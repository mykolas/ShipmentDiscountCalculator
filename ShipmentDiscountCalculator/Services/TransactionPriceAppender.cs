using System;
using System.Collections.Generic;
using System.Globalization;
using ShipmentDiscountCalculator.Entities;
using ShipmentDiscountCalculator.Enums;

namespace ShipmentDiscountCalculator.Services
{
    public class TransactionPriceAppender
    {
        private readonly IDiscountCalculator _discountCalculator;
        private readonly IDictionary<(ShipmentProvider, ShipmentSize), double> _prices;
        private readonly string _dateFormat;

        public TransactionPriceAppender(
            IDiscountCalculator discountCalculator,
            IDictionary<(ShipmentProvider, ShipmentSize), double> prices,
            string dateFormat)
        {
            _discountCalculator = discountCalculator;
            _prices = prices;
            _dateFormat = dateFormat;
        }

        public string Append(string line)
        {
            var transaction = ParseTransaction(line);
            if (transaction == null)
            {
                return $"{line} Ignored";
            }

            var price = _prices[(transaction.Provider, transaction.Size)];
            var discount = _discountCalculator.GetDiscount(transaction);
            var finalPrice = price - discount;

            return $"{line} {finalPrice:0.00} {(discount > 0 ? $"{discount:0.00}" : "-")}";

        }

        private Transaction ParseTransaction(string line)
        {
            var values = line?.Split(" ");

            if (values?.Length == 3
                && DateTime.TryParseExact(values[0], _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                && Enum.TryParse<ShipmentSize>(values[1], out var size)
                && Enum.TryParse<ShipmentProvider>(values[2], out var provider))
            {
                return new Transaction { Date = date, Size = size, Provider = provider };
            }

            return null;
        }
    }
}
