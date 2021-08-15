using BookStore_App.Contracts;
using BookStore_App.Data;
using BookStore_App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Controllers
{
    public class ToDoCController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToDoListService  _toDoListService;
        public ToDoCController(IToDoListService toDoListService, ApplicationDbContext context)
        {
            _context = context;
            _toDoListService = toDoListService;
        }

        // GET: api/ToDo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetToDoList()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/ToDo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetToDo(int id)
        {
            var toDo = await _context.Books.FindAsync(id);

            if (toDo == null)
            {
                return NotFound();
            }

            return toDo;
        }

        // PUT: api/ToDo/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo(int id, Book toDo)
        {
            if (id != toDo.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDo
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Book>> PostToDo(Book toDo)
        {
            _context.Books.Add(toDo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDo", new { id = toDo.Id }, toDo);
        }

        // DELETE: api/ToDo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteToDo(int id)
        {
            var toDo = await _context.Books.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }

            _context.Books.Remove(toDo);
            await _context.SaveChangesAsync();

            return toDo;
        }
        [HttpGet]
        [Route("Getv2")]
        public async Task<ActionResult<PaginatedList<Book>>> Get(int? pageNumber, string sortField, string sortOrder)
        {
            var list = await _toDoListService.GetList(pageNumber, sortField, sortOrder);
            return list;
        }
        private bool ToDoExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
