using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.PayJunction
{
    public class SmartTerminalPaymentRequestStatus
    {
        [JsonProperty("status")]
        public string StatusText { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        public override string ToString()
        {
            return this.StatusText + 
                (!String.IsNullOrEmpty(this.TransactionId) ? " (" + this.TransactionId + ")" : null);
        }
    }
}
