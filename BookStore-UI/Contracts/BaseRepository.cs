using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Contracts
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _client;
        public BaseRepository(IHttpClientFactory httpClient)
        {
            _client = httpClient;
        }
        public async Task<T> Create(string url, T obj, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _client.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return null;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
                return false;
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = _client.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)

                return true;
            return false;


        }

        public async Task<T> GetAll(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _client.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return null;
        }

        public async Task<T> GetById(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+id);
            var client = _client.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return null;
        }

        public async Task<bool> Update(string url, T obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);

            if (obj == null)
                return false;
            request.Content = new StringContent(JsonConvert.SerializeObject(obj)
                , Encoding.UTF8, "application/json");
            var client = _client.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }
    }
}
