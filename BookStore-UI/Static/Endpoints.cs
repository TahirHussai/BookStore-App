using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Static
{
    public static class Endpoints
    {
        public static string BaseUrl = "http://localhost:58237";
        public static string AuthorsEndPoint = $"{BaseUrl}api/authors";
        public static string BooksEndPoint = $"{BaseUrl}api/books";
    }
}
