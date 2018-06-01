using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.PayJunctionApi
{
    public class ErrorsCollection
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; } = new List<Error>();

        [JsonProperty("help")]
        public string HelpUrl { get; set; }

        public override string ToString()
        {
            return "Errors: " + this.Errors.Count + 
                (!String.IsNullOrEmpty(this.HelpUrl) ? " (" + this.HelpUrl + ")" : null); 
        }
    }
}
