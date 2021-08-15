using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookStore_UI.Services
{
    public class BookRepository: BaseRepository<Book>,IBookRepository
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStoeageService;
        public BookRepository(IHttpClientFactory httpClientFactory, ILocalStorageService localStoeageService) : base(httpClientFactory, localStoeageService)
        {
            _client = httpClientFactory;
            _localStoeageService = localStoeageService;
        }
        //public async Task<PaginatedList<Book>> GetPagedResult(int? pageNumber, string sortField, string sortOrder)
        //{
        //    var response = await _client.GetAsync($"/BlazorDataService/ToDoList/Getv2?pageNumber={pageNumber}&sortField={sortField}&sortOrder={sortOrder}");
        //    response.EnsureSuccessStatusCode();

        //    using var responseStream = await response.Content.ReadAsStreamAsync();
        //    var result = await JsonSerializer.DeserializeAsync<PaginatedList<Book>>(responseStream, new JsonSerializerOptions
        //    {
        //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        //        PropertyNameCaseInsensitive = true,
        //    });
        //    return result;
        //}

    }
}
