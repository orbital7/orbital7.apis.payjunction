using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Apis.PayJunction
{
    public class Terminal
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", this.TerminalId, this.Nickname);
        }
    }
}
