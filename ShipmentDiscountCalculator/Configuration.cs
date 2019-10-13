using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;

namespace ShipmentDiscountCalculator
{
    public interface IConfiguration
    {
        string DateFormat { get; }

        IDictionary<(ShipmentType, ShipmentSize), double> DefaultShippingPrices { get; }

        // Accumulated Discount Limit Rule settings
        double MaximumMonthlyDiscount { get; }
        
        // Lowest Price Among Providers rule settings
        ShipmentSize LowestPriceAmongProvidersSize { get; }

        // Repeated Size rule settings
        int RepeatedSizeRuleRepetitionCount { get; }
        ShipmentSize RepeatedSizeRuleSize { get; }
        ShipmentType RepeatedSizeRuleProvider { get; }
    }

    public class Configuration : IConfiguration
    {
        public string DateFormat => "yyyy-MM-dd";

        public IDictionary<(ShipmentType, ShipmentSize), double> DefaultShippingPrices => new Dictionary<(ShipmentType, ShipmentSize), double>()
        {
            {(ShipmentType.LP, ShipmentSize.S), 1.50 },
            {(ShipmentType.LP, ShipmentSize.M), 4.90 },
            {(ShipmentType.LP, ShipmentSize.L), 6.90 },
            {(ShipmentType.MR, ShipmentSize.S), 2 },
            {(ShipmentType.MR, ShipmentSize.M), 3 },
            {(ShipmentType.MR, ShipmentSize.L), 4 }
        };
        
        public double MaximumMonthlyDiscount => 10;

        public ShipmentSize LowestPriceAmongProvidersSize => ShipmentSize.S;

        public int RepeatedSizeRuleRepetitionCount => 3;
        public ShipmentSize RepeatedSizeRuleSize => ShipmentSize.L;
        public ShipmentType RepeatedSizeRuleProvider => ShipmentType.LP;
    }
}
