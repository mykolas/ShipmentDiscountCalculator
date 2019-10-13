using System;
using ShipmentDiscountCalculator.Enums;

namespace ShipmentDiscountCalculator.Entities
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public ShipmentSize Size { get; set; }
        public ShipmentType Type { get; set; }
    }
}
