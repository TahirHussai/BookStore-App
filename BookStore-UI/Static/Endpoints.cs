using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Static
{
    public static class Endpoints
    {
        public static string BaseUrl = "https://localhost:44352/";
        public static string AuthorsEndPoint = $"{BaseUrl}api/authors";
        public static string BooksEndPoint = $"{BaseUrl}api/books";
        public static string RegisterEndpoint = $"{BaseUrl}api/Users/register/";
        public static string LoginEndPoint = $"{BaseUrl}api/Users/login";
    }
}
