using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ValueThing
{
    class Searcher
    {
        public delegate void Callback(JSearch[] jSearchs);

        private string keyword;
        private Callback CallbackMethod;

        public Searcher( string keyword, Callback callback )
        {
            this.keyword = keyword;
            this.CallbackMethod = callback;
        }

        public void Search()
        {
            string query = "http://122.117.109.65/webapi/search?keyword=" + keyword;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(query));
            request.BeginGetResponse(WebResponseCallback, request);
        }

        private void WebResponseCallback(IAsyncResult result)
        {
            JSearch[] jSearchs;
            try
            {
                HttpWebRequest request = ((HttpWebRequest)result.AsyncState);
                StreamReader sr = new StreamReader(request.EndGetResponse(result).GetResponseStream());
                string data = sr.ReadToEnd();
                jSearchs = JsonConvert.DeserializeObject<JSearch[]>(data);               
            }
            catch
            {
                jSearchs = new JSearch[0];
            }
            CallbackMethod(jSearchs);
        } 
    }
}
