using BookStore_App.Contracts;
using BookStore_App.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Services
{
    public class BookRepository : IBookRepositorycs
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<bool> Create(Book entity)
        {
            await _context.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            _context.Books.Remove(entity);
            return await Save();
        }

        public async Task<IList<Book>> GetAll()
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }

        public async Task<Book> GetById(int id)
        {
            var books = await _context.Books.FindAsync(id);
            return books;
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> Update(Book entity)
        {
            _context.Books.Update(entity);
            return await Save();
        }
    }
}
