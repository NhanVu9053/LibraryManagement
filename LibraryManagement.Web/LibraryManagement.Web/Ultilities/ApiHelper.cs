using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Ultilities
{
    public static class ApiHelper <T> where T : class
    {
        public static T HttpGetAsync(string apiName)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(@$"{Common.apiUrl}/{apiName}");
            httpWebRequest.Method = "GET";
            var response = httpWebRequest.GetResponse();
            {
                string responseData;
                Stream responseStream = response.GetResponseStream();
                try
                {
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        responseData = sr.ReadToEnd();
                    }
                }
                finally
                {
                    ((IDisposable)responseStream).Dispose();
                }
                return JsonConvert.DeserializeObject<T>(responseData);

            }
        }

        public static T HttpAsync(string apiName, string method, object model)
        {
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(@$"{Common.apiUrl}/{apiName}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;
            using (var streamWrite = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(model);
                streamWrite.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static T HttpPatchAsync(string apiName)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(@$"{Common.apiUrl}/{apiName}");
            httpWebRequest.Method = "PATCH";
            var response = httpWebRequest.GetResponse();
            {
                string responseData;
                Stream responseStream = response.GetResponseStream();
                try
                {
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        responseData = sr.ReadToEnd();
                    }
                }
                finally
                {
                    ((IDisposable)responseStream).Dispose();
                }
                return JsonConvert.DeserializeObject<T>(responseData);

            }
        }
        public static T HttpPostAsync(string apiName)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(@$"{Common.apiUrl}/{apiName}");
            httpWebRequest.Method = "POST";
            var response = httpWebRequest.GetResponse();
            {
                string responseData;
                Stream responseStream = response.GetResponseStream();
                try
                {
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        responseData = sr.ReadToEnd();
                    }
                }
                finally
                {
                    ((IDisposable)responseStream).Dispose();
                }
                return JsonConvert.DeserializeObject<T>(responseData);

            }
        }
        public static T HttpDeleteAsync(string apiName)
        {
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(@$"{Common.apiUrl}/{apiName}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "DELETE";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
