using ShipmentDiscountCalculator.DiscountRules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShipmentDiscountCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var rules = GetRules();
            var discountCalculator = new DiscountCalculator(rules);
            var transactionPriceAppender = new TransactionPriceAppender(
                discountCalculator,
                Configuration.DefaultShippingPrices,
                Configuration.DateFormat);

            var filePath = args.FirstOrDefault() ?? @"C:\Code\input.txt";
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                Console.WriteLine(transactionPriceAppender.Append(line));
            }
        }

        private static IList<IDiscountRule> GetRules() => new List<IDiscountRule>
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
