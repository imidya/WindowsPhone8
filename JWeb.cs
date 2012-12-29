using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueThing
{
    class JWeb
    {
        [JsonProperty(PropertyName = "GsearchResultClass")]
        public string GsearchResultClass;

        [JsonProperty(PropertyName = "visibleUrl")]
        public string VisibleUrl;

        [JsonProperty(PropertyName = "titleNoFormatting")]
        public string TitleNoFormatting;

        [JsonProperty(PropertyName = "title")]
        public string Title;

        [JsonProperty(PropertyName = "url")]
        public string Url;

        [JsonProperty(PropertyName = "cacheUrl")]
        public string CacheUrl;

        [JsonProperty(PropertyName = "unescapedUrl")]
        public string UnescapedUrl;

        [JsonProperty(PropertyName = "content")]
        public string Content;
    }
}
