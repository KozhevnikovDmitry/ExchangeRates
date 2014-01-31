using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using ExchangeRates.BL.Interface;

namespace ExchangeRates.BL
{
    /// <summary>
    /// <see cref="HttpClient"/> adapter, that provides deserialized rate sources from https://openexchangerates.org/
    /// </summary>
    internal class RateClient : IRateClient
    {
        private readonly AppId _appId;

        /// <summary>
        /// Constructs example of <see cref="RateClient"/>
        /// </summary>
        /// <param name="appId">Access token</param>
        public RateClient(AppId appId)
        {
            _appId = appId;
        }

        /// <summary>
        /// Provides rates source by <paramref name="date"/>
        /// </summary>
        public RateSourceData GetRate(DateTime date)
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
                    return (RateSourceData)new DataContractJsonSerializer(typeof(RateSourceData)).ReadObject(json);
                }
            }

            return null;
        }
    }
}