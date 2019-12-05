using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.PayJunction
{
    public class TransactionResultFailureException : Exception
    {
        public TransactionResult TransactionResult { get; set; }

        public TransactionResultFailureException()
            : base()
        {

        }

        public TransactionResultFailureException(TransactionResult transactionResult)
            : base(String.Format("Transaction {0}", transactionResult.Response.Message))
        {
            this.TransactionResult = transactionResult;
        }
    }
}
