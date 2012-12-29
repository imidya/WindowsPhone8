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
    class Rater
    {
        public delegate void Callback(JRate jRate);

        private string keyword;
        private Callback CallbackMethod;

        public Rater(string keyword, Callback callback)
        {
            this.keyword = keyword;
            this.CallbackMethod = callback;
        }

        public void Rate()
        {
            string query = "http://122.117.109.65/webapi/rate?keyword=" + keyword;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(query));
            request.BeginGetResponse(WebResponseCallback, request);
        }

        private void WebResponseCallback(IAsyncResult result)
        {
            JRate jRate;
            try
            {
                HttpWebRequest request = ((HttpWebRequest)result.AsyncState);
                StreamReader sr = new StreamReader(request.EndGetResponse(result).GetResponseStream());
                string data = sr.ReadToEnd();
                jRate = JsonConvert.DeserializeObject<JRate>(data);               
            }
            catch
            {
                jRate = new JRate();
            }
            CallbackMethod(jRate);
        }
    }
}
