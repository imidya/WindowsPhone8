using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ValueThing
{
    class Poster
    {
        public delegate void Callback(JPosts jPosts);

        private string keyword;
        private Callback CallbackMethod;

        public Poster(string keyword, Callback callback)
        {
            this.keyword = keyword;
            this.CallbackMethod = callback;
        }

        public void Get()
        {
            string query = "http://122.117.109.65/webapi/google?keyword=" + keyword;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(query));
            request.BeginGetResponse(WebResponseCallback, request);
        }

        private void WebResponseCallback(IAsyncResult result)
        {
            JPosts jPosts;
            try
            {
                HttpWebRequest request = ((HttpWebRequest)result.AsyncState);
                StreamReader sr = new StreamReader(request.EndGetResponse(result).GetResponseStream());
                string data = sr.ReadToEnd();
                jPosts = JsonConvert.DeserializeObject<JPosts>(data);               
            }
            catch
            {
                jPosts = new JPosts();
            }
            CallbackMethod(jPosts);
        }
    }
}
