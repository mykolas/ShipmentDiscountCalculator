using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ShipmentDiscountCalculator.DiscountRules;
using ShipmentDiscountCalculator.Enums;

namespace ShipmentDiscountCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new Configuration();

            var rules = GetRules(configuration);
            var discountCalculator = new DiscountCalculator(rules);
            var transactionPriceAppender = new TransactionPriceAppender(
                discountCalculator,
                configuration.DefaultShippingPrices,
                configuration.DateFormat);

            var filePath = args.FirstOrDefault() ?? @"C:\Code\input.txt";
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                Console.WriteLine(transactionPriceAppender.Append(line));
            }
        }

        private static IList<IDiscountRule> GetRules(IConfiguration configuration) => new List<IDiscountRule>
        {
            new LowestPriceAmongProvidersRule(
                configuration.LowestPriceAmongProvidersSize,
                configuration.DefaultShippingPrices),
            new RepeatedSizeRule(
                configuration.RepeatedSizeRuleSize,
                ShipmentType.LP,
                configuration.RepeatedSizeRuleRepetitionCount,
                configuration.DefaultShippingPrices),
            new AccumulatedDiscountLimitRule(configuration.MaximumMonthlyDiscount)
        };
    }
}
