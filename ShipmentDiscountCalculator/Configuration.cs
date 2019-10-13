using ShipmentDiscountCalculator.Enums;
using System.Collections.Generic;

namespace ShipmentDiscountCalculator
{
    public static class Configuration
    {
        public static string DateFormat => "yyyy-MM-dd";

        public static IDictionary<(ShipmentProvider, ShipmentSize), double> DefaultShippingPrices =>
            new Dictionary<(ShipmentProvider, ShipmentSize), double>
            {
                {(ShipmentProvider.LP, ShipmentSize.S), 1.50},
                {(ShipmentProvider.LP, ShipmentSize.M), 4.90},
                {(ShipmentProvider.LP, ShipmentSize.L), 6.90},
                {(ShipmentProvider.MR, ShipmentSize.S), 2},
                {(ShipmentProvider.MR, ShipmentSize.M), 3},
                {(ShipmentProvider.MR, ShipmentSize.L), 4}
            };

        // Accumulated Discount Limit Rule settings
        public static double MaximumMonthlyDiscountRuleLimit => 10;

        // Lowest Price Among Providers Rule settings
        public static ShipmentSize LowestPriceAmongProvidersRuleSize => ShipmentSize.S;

        // Repeated Size Rule settings
        public static int RepeatedSizeRuleRepetitionCount => 3;
        public static ShipmentSize RepeatedSizeRuleSize => ShipmentSize.L;
        public static ShipmentProvider RepeatedSizeRuleProvider => ShipmentProvider.LP;
    }
}
