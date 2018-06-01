using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.PayJunctionApi
{
    public class ResultsCollection<T> where T : class
    {
        [JsonProperty("results")]
        public List<T> Results { get; set; } = new List<T>();

        public override string ToString()
        {
            return "Results: " + this.Results.Count;
        }
    }
}
