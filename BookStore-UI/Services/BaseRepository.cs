﻿
using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookStore_UI.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage; 
        public BaseRepository(IHttpClientFactory client, ILocalStorageService localStorageService)
        {
            _client = client;
            _localStorage = localStorageService;
        }

        public async Task<bool> Create(string url, T obj)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (obj==null)
            {
                return false;
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(obj),
               Encoding.UTF8, "application/json");
            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode==System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id<0)
            {
                return false;
            }
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);
          
            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<T> Get(string url, int id)
        {
            
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            return null;
        }
        public async Task<PaginatedList<T>> GetList(string url, int? pageNumber, string sortField, string sortOrder)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url  + "?pageNumber="+pageNumber + "&sortField=" + sortField+ "&sortOrder="+sortOrder);

            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage httpResponse = await client.SendAsync(request);

            using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
            var result = await System.Text.Json.JsonSerializer.DeserializeAsync<PaginatedList<T>>(responseStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }
        public async Task<IList<T>> Get(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var client = _client.CreateClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", await GetBearerToken());
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IList<T>>(content);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
               
            }
        }

        public async Task<bool> Update(string url, T obj,int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url+id);
            if (obj == null)
            {
                return false;
            }
            request.Content = new StringContent(JsonConvert.SerializeObject(obj),
                Encoding.UTF8,"application/json");
            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("bearer", await GetBearerToken());
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<string> GetBearerToken()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

       
    }
}
