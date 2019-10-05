using System.Collections.Generic;

namespace ShipmentDiscountCalculator
{
    public interface IConfiguration
    {
        IDictionary<(ShipmentType, ShipmentSize), double> DefaultShippingPrices { get; }
        double MaximumMonthlyDiscount { get; }
    }

    public class Configuration : IConfiguration
    {
        public IDictionary<(ShipmentType, ShipmentSize), double> DefaultShippingPrices => new Dictionary<(ShipmentType, ShipmentSize), double>() { };
        public double MaximumMonthlyDiscount => 10;
    }
}
