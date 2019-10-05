using System;
using System.Collections.Generic;
using System.Text;

namespace ShipmentDiscountCalculator.Services
{
    public interface ITransactionEstimationService
    {
        EstimatedTransaction Estimate(Transaction transaction);
    }

    public class TransactionEstimationService : ITransactionEstimationService
    {
        public EstimatedTransaction Estimate(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
