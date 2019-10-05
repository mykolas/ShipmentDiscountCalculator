using System.Collections.Generic;

namespace ShipmentDiscountCalculator.Services
{
    public interface IEstimatedTransactionsWriterService
    {
        void Write(IEnumerable<EstimatedTransaction> estimatedTransactions);
    }
}
