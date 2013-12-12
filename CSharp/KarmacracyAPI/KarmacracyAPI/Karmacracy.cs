using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace KarmacracyAPI
{
    public class Karmacracy
    {
        /// <summary>
        /// Karmacracy constructor. It needs an API key to work
        /// </summary>
        /// <param name="appKey">API key</param>
        public Karmacracy(string appKey)
        {
            mAppKey = appKey;
        }

        /// <summary>
        /// Allows to query, search and explore all the kcys from Karmacracy.com.
        /// </summary>
        /// <param name="start">First kcy to get</param>
        /// <param name="num">Total number of kcys</param>
        /// <param name="type">Kcy type</param>
        /// <returns></returns>
        public List<Kcy> GetKcys(int start = 1, int num = 10, KcyType type = KcyType.Kclicks)
        {
            string url = string.Format("http://karmacracy.com/api/v1/world?appkey={0}&from={1}&num={2}&type={3}",
                mAppKey, start, num, (int)type);

            RootObject result = GetRootObjectFromUrl(url);

            return result.data.kcy;
        }

        private RootObject GetRootObjectFromUrl(string url)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            WebClient client = new WebClient();
            Stream data = client.OpenRead(url);
            var result = (RootObject)serializer.ReadObject(data);
            data.Close();
            return result;
        }

        public List<Nut> GetNuts(string username)
        {
            string url = string.Format("http://karmacracy.com/api/v1/awards/{1}?appkey={0}", mAppKey, username);
            RootObject result = GetRootObjectFromUrl(url);
            return result.data.nut;
        }

        public Nut GetNut(string username, string nut)
        {
            string url = string.Format("http://karmacracy.com/api/v1/awards/{1}/nut/{2}?appkey={0}", mAppKey, username, nut);
            RootObject result = GetRootObjectFromUrl(url);
            return result.data.nut[0];
        }

        private string EncodeURIComponent(string component)
        {
            //Todo: What to do here?
            return component;
        }

        private string GetUrl(string method, Dictionary<string, string> parameters)
        {
            string url = mBaseUrl;
            string serializedParams;

            //I we needed to add another case, this code needs to be modified, breaks OCP. Needs refactor
            switch (method)
            {
                case "key":
                    url += method + "/";
                    parameters.Remove("k");
                    parameters["p"] = EncodeURIComponent(parameters["p"]);
                    break;
                case "key:check":
                    method = method.Replace(":", "/");
                    url += method + "/";
                    parameters["key"] = parameters["k"]; //I'm sure there is a way to access dictionaries -> Check nancyfx
                    parameters.Remove("k");
                    break;
                case "user":
                case "awards":
                    url += string.Format("{0}/{1}", method, parameters["u"]);
                    parameters.Remove("u");
                    break;
                case "awards:nut":
                    if (!parameters.ContainsKey("n"))
                        throw new ArgumentException();
                    var splittedMethod 
                        = method.Split(new [] {":"}, StringSplitOptions.RemoveEmptyEntries);
                    url += string.Format("{0}/{1}/{2}/{3}", splittedMethod[0], parameters["u"],  splittedMethod[1], parameters["n"]);
                    parameters.Remove("u");
                    parameters.Remove("n");
                    break;
                case "networks:fbpages":
                    method = method.Replace(":", "/");
                    url += method + "/";
                    break;
                case "stats:evolution":
                case "stats:relevance":
                    method = method.Replace(":", "/");
                    url += method + "/";
                    parameters.Remove("k");
                    break;
                case "kcy":
                case "world":
                    url += string.Format("{0}/{1}", method, parameters["kcy"]);
                    parameters.Remove("kcy");
                    parameters.Remove("u");
                    parameters.Remove("k");
                    break;
                case "networks":
                case "domains":
                case "rank":
                    url += method + "/";
                    break;
                case "share":
                    url += method + "/";
                    parameters["txt"] = EncodeURIComponent(parameters["txt"]);
                    break;
                case "shortLink":
                    url = "http://kcy.me/api/";
                    parameters["format"] = "json";
                    parameters["key"] = parameters["k"];
                    parameters["url"] = EncodeURIComponent(parameters["url"]);
                    parameters.Remove("k");
                    break;
                case "firewords":
                    url += method + "/";
                    parameters["format"] = "json";
                    break;
            }

            serializedParams = SerializeObject(parameters);
            if (serializedParams != string.Empty)
                url += "?" + serializedParams;
                    
            return url;
        }

        private string SerializeObject(Dictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in parameters.Keys)
            {
                builder.Append(string.Format("{0}={1}", item, parameters[item]));
            }
            return builder.ToString();
        }

        private string mBaseUrl = "http://karmacracy.com/api/v1/";
        private string mAppKey;
    }

    public class Human
    {
        public string username { get; set; }
        public string kcyrank { get; set; }
        public string img { get; set; }
    }

    public class Person
    {
        public string username { get; set; }
        public string userimg { get; set; }
        public string clicks { get; set; }
    }

    public class Kcy
    {
        public string id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imgUser { get; set; }
        public string image { get; set; }
        public string time { get; set; }
        public string user { get; set; }
        public string clicks { get; set; }
        public string weight { get; set; }
        public List<Person> people { get; set; }
    }

    public class Nut
    {
        public string id { get; set; }
        public string name { get; set; }
        public string imageSmall { get; set; }
        public string imageBig { get; set; }
        public string level { get; set; }
        public string dateReceivedOrLast { get; set; }
        public string number { get; set; }
        public string dateSince { get; set; }
        public string history { get; set; }
        public string description { get; set; }
        public string flg_type { get; set; }
        public string nrKcys { get; set; }
        public string nrMyKcys { get; set; }
        public string nrHumans { get; set; }
        public List<Human> humans { get; set; }
    }
     
    public class Data
    {
        public List<Kcy> kcy { get; set; }        
        public int numnuts { get; set; }        
        public List<Nut> nut { get; set; }
    }

    public class RootObject
    {
        public Data data { get; set; }
    }
}
