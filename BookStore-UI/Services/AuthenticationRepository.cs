using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;
using BookStore_UI.Providers;
using BookStore_UI.Static;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Services
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IHttpClientFactory _client;
       
         private readonly ILocalStorageService _localStorage;
        private readonly ApiAuthenticationStateProvider _apiAuthenticationStateProvider;
        public AuthenticationRepository(IHttpClientFactory client,
           ILocalStorageService localStorage, ApiAuthenticationStateProvider apiAuthenticationStateProvider
            )
        {
            _client = client;
           _localStorage = localStorage;
            _apiAuthenticationStateProvider = apiAuthenticationStateProvider;
        }

        //public async Task<bool> Login(LoginModel user)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Post
        //        , Endpoints.LoginEndPoint)
        //    {
        //        Content = new StringContent(JsonConvert.SerializeObject(user)
        //        , Encoding.UTF8, "application/json")
        //    };

        //    var client = _client.CreateClient();
        //    HttpResponseMessage response = await client.SendAsync(request);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return false;
        //    }
        //    return true;
        //    // Install Blazored.localStorage NugetPackage to store Token Response
        //    // after installation configure in startup.cs class
        //}

        public async Task<bool> Login(LoginModel user)  
        {
            var request = new HttpRequestMessage(HttpMethod.Post
               , Endpoints.LoginEndPoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(user)
                , Encoding.UTF8, "application/json");

            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
           
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);
            
            // Install Blazored.localStorage NugetPackage to store Token Response
            // after installation configure in startup.cs class
            //Store Token
            await _localStorage.SetItemAsync("authToken", token.Token);
            await _localStorage.SetItemAsync("LoginName", user.EmailAddress);

            //Change auth state of app
            await ((ApiAuthenticationStateProvider)_apiAuthenticationStateProvider).LoggedIn(user.EmailAddress);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token.Token);
            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_apiAuthenticationStateProvider)
                .LoggedOut();
        }

        public async Task<bool> Register(RegistrationModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post
                , Endpoints.RegisterEndpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user)
                , Encoding.UTF8, "application/json")
            };

            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
       
    }
}
