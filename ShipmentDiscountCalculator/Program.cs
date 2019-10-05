using System.Linq;
using ShipmentDiscountCalculator.Services;

namespace ShipmentDiscountCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var transactionReader = new TransactionsFileReaderService();
            var transactionWriter = new EstimatedTransactionsFileWriterService();
            var transactionEstimator = new TransactionEstimationService();

            var transactions = transactionReader.Read();
            
            var estimatedTransactions = transactions.Select(transactionEstimator.Estimate);
            
            transactionWriter.Write(estimatedTransactions);
        }
    }
}
