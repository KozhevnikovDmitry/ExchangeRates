using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using ExchangeRates.BL.Interface;

namespace ExchangeRates.BL
{
    internal class AppId
    {
        public string Value { get; set; }
    }

    internal class RateClient : IRateClient
    {
        private readonly AppId _appId;

        public RateClient(AppId appId)
        {
            _appId = appId;
            _appId = new AppId { Value = "9ec36de63b284d7dbc50f8a7d278ebfd" };
        }

        public RateResponce GetRate(DateTime date)
        {
            var task = Get(date.ToString("yyyy-MM-dd"));
            task.Wait();
            return task.Result;
        }

        private async Task<RateResponce> Get(string date)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://openexchangerates.org/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var uri = string.Format("api/historical/{0}.json?app_id={1}", date, _appId.Value);
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStreamAsync();
                    return (RateResponce)new DataContractJsonSerializer(typeof(RateResponce)).ReadObject(json);
                }
            }

            return null;
        }
    }
}