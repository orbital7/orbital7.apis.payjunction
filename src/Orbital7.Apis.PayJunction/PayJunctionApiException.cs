using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.PayJunction
{
    public class PayJunctionApiException : Exception
    {
        public ErrorsCollection ErrorsCollection { get; set; }

        public PayJunctionApiException(string message, ErrorsCollection errorsCollection, Exception innerException = null)
            : base(message, innerException)
        {
            this.ErrorsCollection = errorsCollection;
        }
    }
}
