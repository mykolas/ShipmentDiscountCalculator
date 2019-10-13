using ShipmentDiscountCalculator.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ShipmentDiscountCalculator
{
    public class TransactionPriceAppender : ITransactionPriceAppender
    {
        private readonly IDiscountCalculator _discountCalculator;
        private readonly IDictionary<(ShipmentType, ShipmentSize), double> _prices;
        private readonly string _dateFormat;

        public string DateFormat1 { get; }

        public TransactionPriceAppender(
            IDiscountCalculator discountCalculator,
            IDictionary<(ShipmentType, ShipmentSize), double> prices,
            string dateFormat)
        {
            _discountCalculator = discountCalculator;
            _prices = prices;
            _dateFormat = dateFormat;
        }

        public string Append(string line)
        {
            var transaction = ParseTransaction(line);
            if (transaction != null)
            {
                var price = _prices[(transaction.Type, transaction.Size)];
                var discount = _discountCalculator.GetDiscount(transaction);
                var finalPrice = price - discount;

                return $"{line} {finalPrice:0.00} {(discount == 0 ? "-" : $"{discount:0.00}")}";
            }
            else
            {
                return $"{line} Ignored";
            }
        }

        private Transaction ParseTransaction(string line)
        {
            var values = line?.Split(" ");

            if (values?.Length == 3
                && DateTime.TryParseExact(values[0], _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                && Enum.TryParse<ShipmentSize>(values[1], out ShipmentSize size)
                && Enum.TryParse<ShipmentType>(values[2], out ShipmentType type))
            {
                return new Transaction { Date = date, Size = size, Type = type };
            }

            return null;
        }
    }
}
