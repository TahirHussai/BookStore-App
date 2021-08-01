using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore_UI.Services
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStoeageService;
        public AuthorRepository(IHttpClientFactory httpClientFactory, ILocalStorageService localStoeageService) :base(httpClientFactory, localStoeageService)
        {
            _client = httpClientFactory;
            _localStoeageService = localStoeageService;
        }
       
    }
}
