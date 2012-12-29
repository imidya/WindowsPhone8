using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueThing
{
    class JPosts
    {
        [JsonProperty(PropertyName = "blog")]
        public JBlog[] Blogs;

        [JsonProperty(PropertyName = "web")]
        public JWeb[] Webs;
    }
}
