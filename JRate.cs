using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueThing
{
    class JRate
    {
        [JsonProperty(PropertyName = "keyword")]
        public string Keyword;

        [JsonProperty(PropertyName = "rate")]
        public int Rate;
    }
}
