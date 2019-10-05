using System;
using System.Collections.Generic;
using System.Text;

namespace ShipmentDiscountCalculator.Services
{
    public interface ITransactionsReaderService
    {
        IEnumerable<Transaction> Read();
    }
}
