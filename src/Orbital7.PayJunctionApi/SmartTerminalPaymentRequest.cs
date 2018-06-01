using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.PayJunctionApi
{
    public class SmartTerminalPaymentRequest
    {
        [JsonProperty("requestPaymentId")]
        public string RequestPaymentId { get; set; }
    }
}
