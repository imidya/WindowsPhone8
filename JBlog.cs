using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValueThing
{
    class JBlog
    {
        [JsonProperty(PropertyName = "blogUrl")]
        public string BlogUrl;

        [JsonProperty(PropertyName = "GsearchResultClass")]
        public string GsearchResultClass;

        [JsonProperty(PropertyName = "author")]
        public string Author;

        [JsonProperty(PropertyName = "publishedDate")]
        public string PublishedDate;

        [JsonProperty(PropertyName = "titleNoFormatting")]
        public string TitleNoFormatting;

        [JsonProperty(PropertyName = "content")]
        public string Content;

        [JsonProperty(PropertyName = "postUrl")]
        public string PostUrl;

        [JsonProperty(PropertyName = "title")]
        public string Title;
    }
}
