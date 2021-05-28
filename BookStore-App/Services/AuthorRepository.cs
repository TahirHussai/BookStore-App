using BookStore_App.Contracts;
using BookStore_App.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<bool> Create(Author entity)
        {
          await _context.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            _context.Authors.Remove(entity);
            return await Save();
        }

        public async Task<List<Author>> GetAll()
        {
            var authors = await _context.Authors.ToListAsync();
            return authors;
        }

        public async Task<Author> GetById(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author;
        }

        public async Task<bool> Save()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(Author entity)
        {
            _context.Authors.Update(entity);
            return await Save();
        }
    }
}
