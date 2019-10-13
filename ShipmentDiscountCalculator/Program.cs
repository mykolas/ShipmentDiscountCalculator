using ShipmentDiscountCalculator.DiscountRules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShipmentDiscountCalculator.Services;

namespace ShipmentDiscountCalculator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var filePath = args.FirstOrDefault() ?? "input.txt";
            
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine($"Provided input file ({filePath}) does not exist.");
                return;
            }

            var discountCalculator = new DiscountCalculator(Rules);
            
            var transactionPriceAppender = new TransactionPriceAppender(
                discountCalculator,
                Configuration.DefaultShippingPrices,
                Configuration.DateFormat);

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                Console.WriteLine(transactionPriceAppender.Append(line));
            }
        }

        private static IList<IDiscountRule> Rules => new List<IDiscountRule>
        {
            new LowestPriceAmongProvidersRule(
                Configuration.LowestPriceAmongProvidersRuleSize,
                Configuration.DefaultShippingPrices),

            new RepeatedSizeRule(
                Configuration.RepeatedSizeRuleSize,
                Configuration.RepeatedSizeRuleProvider,
                Configuration.RepeatedSizeRuleRepetitionCount,
                Configuration.DefaultShippingPrices),
            
            new AccumulatedDiscountLimitRule(Configuration.MaximumMonthlyDiscountRuleLimit)
        };
    }
}
