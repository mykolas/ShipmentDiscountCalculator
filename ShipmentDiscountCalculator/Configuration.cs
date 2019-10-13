using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;

namespace ShipmentDiscountCalculator
{
    public static class Configuration
    {
        public static string DateFormat => "yyyy-MM-dd";

        public static IDictionary<(ShipmentType, ShipmentSize), double> DefaultShippingPrices =>
            new Dictionary<(ShipmentType, ShipmentSize), double>
            {
                {(ShipmentType.LP, ShipmentSize.S), 1.50},
                {(ShipmentType.LP, ShipmentSize.M), 4.90},
                {(ShipmentType.LP, ShipmentSize.L), 6.90},
                {(ShipmentType.MR, ShipmentSize.S), 2},
                {(ShipmentType.MR, ShipmentSize.M), 3},
                {(ShipmentType.MR, ShipmentSize.L), 4}
            };

        // Accumulated Discount Limit Rule settings
        public static double MaximumMonthlyDiscountRuleLimit => 10;

        // Lowest Price Among Providers Rule settings
        public static ShipmentSize LowestPriceAmongProvidersRuleSize => ShipmentSize.S;

        // Repeated Size Rule settings
        public static int RepeatedSizeRuleRepetitionCount => 3;
        public static ShipmentSize RepeatedSizeRuleSize => ShipmentSize.L;
        public static ShipmentType RepeatedSizeRuleProvider => ShipmentType.LP;
    }
}
