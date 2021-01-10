using AutoMapper;
using BookStore_App.Data.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Data.Mapping
{
    public class Maps : Profile
    {
        public Maps() 
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();

        }
    }
}
