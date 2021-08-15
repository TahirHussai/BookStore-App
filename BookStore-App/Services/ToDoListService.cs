using BookStore_App.Contracts;
using BookStore_App.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly ApplicationDbContext _context;

        public ToDoListService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Book>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<PaginatedList<Book>> GetList(int? pageNumber, string sortField, string sortOrder)
        {
            int pageSize = 3;
            var toDoList = _context.Books.OrderByDynamic(sortField, sortOrder.ToUpper());
            return await PaginatedList<Book>.CreateAsync(toDoList.AsNoTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<Book> Get(int id)
        {
            var toDo = await _context.Books.FindAsync(id);
            return toDo;
        }

        public async Task<Book> Add(Book toDo)
        {
            _context.Books.Add(toDo);
            await _context.SaveChangesAsync();
            return toDo;
        }

        public async Task<Book> Update(Book toDo)
        {
            _context.Entry(toDo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return toDo;
        }

        public async Task<Book> Delete(int id)
        {
            var toDo = await _context.Books.FindAsync(id);
            _context.Books.Remove(toDo);
            await _context.SaveChangesAsync();
            return toDo;
        }
    }
}

