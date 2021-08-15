using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookStore_App.Data;
using BookStore_App.Services;
using Microsoft.EntityFrameworkCore;

namespace BookStore_App.Contracts
{
    public interface IToDoListService
    {
        Task<List<Book>> Get();
        Task<PaginatedList<Book>> GetList(int? pageNumber, string sortField, string sortOrder);
        Task<Book> Get(int id);
        Task<Book> Add(Book toDo);
        Task<Book> Update(Book toDo);
        Task<Book> Delete(int id);
    }
}
