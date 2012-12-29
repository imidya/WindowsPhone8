using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueThing
{
    class JSearch
    {
        public string Data = "";

        [JsonProperty(PropertyName = "url")]
        public string URL;

        [JsonProperty(PropertyName = "source")]
        public string Source;

        [JsonProperty(PropertyName = "img")]
        public string Img;

        [JsonProperty(PropertyName = "title")]
        public string Title;

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }
    }
}
