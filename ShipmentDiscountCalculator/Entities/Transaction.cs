using ShipmentDiscountCalculator.Enums;
using System;
using System.Globalization;

namespace ShipmentDiscountCalculator
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public ShipmentSize Size { get; set; }
        public ShipmentType Type { get; set; }
    }
}
