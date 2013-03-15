using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        private string mAppKey;

        public List<Nut> GetNuts(string username)
        {
            string url = string.Format("http://karmacracy.com/api/v1/awards/{1}?appkey={0}", mAppKey, username);
            RootObject result = GetRootObjectFromUrl(url);
            return result.data.nut;
        }
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
