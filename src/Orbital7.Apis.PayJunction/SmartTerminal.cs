using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.PayJunction
{
    public class SmartTerminal
    {
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("smartTerminalId")]
        public string SmartTerminalId { get; set; }

        [JsonProperty("nickName")]
        public string Nickname { get; set; }

        [JsonProperty("created")]
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", this.SmartTerminalId, this.Nickname);
        }
    }
}
