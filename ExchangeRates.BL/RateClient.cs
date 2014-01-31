using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using ExchangeRates.BL.Interface;

namespace ExchangeRates.BL
{
    internal class AppId
    {
        public AppId(string appId)
        {
            Value = appId;
        }

        public string Value { get; set; }
    }

    internal class RateClient : IRateClient
    {
        private readonly AppId _appId;

        public RateClient(AppId appId)
        {
            _appId = appId;
        }

        public RateResponce GetRate(DateTime date)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://openexchangerates.org/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var uri = string.Format("api/historical/{0}.json?app_id={1}", date.ToString("yyyy-MM-dd"), _appId.Value);
                HttpResponseMessage response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStreamAsync().Result;
                    return (RateResponce)new DataContractJsonSerializer(typeof(RateResponce)).ReadObject(json);
                }
            }

            return null;
        }
    }
}