using AutoMapper;
using BookStore_App.Contracts;
using BookStore_App.Data;
using BookStore_App.Data.DTO_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepositorycs  _bookRepositorycs;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        public BooksController(IMapper mapper, IBookRepositorycs  bookRepositorycs, ILoggerService loggerService)
        {
            _bookRepositorycs =  bookRepositorycs;
            _loggerService = loggerService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Books
        /// </summary>
        /// <return>Return List of Books</return>
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                _loggerService.LogInfo("Attempted Get All Books");
                var authors = await _bookRepositorycs.GetAll();
                var res = _mapper.Map<List<BookDTO>>(authors);
                _loggerService.LogInfo("Successfully got All Books");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return InternalError($"{ ex.Message}-{ex.InnerException}");
            }
        }
        /// <summary>
        /// Create An Authors
        /// </summary>
        /// <return>Add Author Authors</return>
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateDto dto)
        {
            try
            {
                _loggerService.LogWarn($"Books Submission Attempted");

                if (dto == null)
                {
                    _loggerService.LogWarn($"Empty Request Submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _loggerService.LogWarn($"Empty Response");
                    return BadRequest(ModelState);
                }
                var books = _mapper.Map<Book>(dto);
                var isuccess = await _bookRepositorycs.Create(books);
                if (!isuccess)
                {
                    return InternalError($"Books creation field");
                }
                _loggerService.LogInfo("Books Operation Performed Successfully");
                return Created("Created", new { books });
            }
            catch (Exception ex)
            {

                return InternalError($"{ ex.Message}-{ex.InnerException}");

            }
        }
        /// <summary>
        /// Update An book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <return>Add  books</return>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookUpdateDto dto)
        {
            _loggerService.LogInfo($"Book with id :{id} Update Attempted");
            try
            {


                if (id < 1 && dto == null && id != dto.Id)
                {
                    _loggerService.LogInfo($"Book with id :{id} Update Attempted");

                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(dto);
                var isuccess = await _bookRepositorycs.Update(book);
                if (!isuccess)
                {
                    return InternalError($"Book updation field");
                }
                _loggerService.LogInfo("Book Updated Successfully");
                return NoContent();
            }

            catch (Exception ex)
            {

                return InternalError($"{ ex.Message}-{ex.InnerException}");

            }

        }
        /// <summary>
        /// Get book
        /// </summary>
        /// <param name="id"></param>
        /// <return>Get By Id book </return>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id = 0)
        {
            try
            {
                _loggerService.LogInfo($"Attempted Get  Book with id :{id}");
                var books = await _bookRepositorycs.GetById(id);
                if (books == null)
                {
                    _loggerService.LogWarn($" Book with id :{id} was not found");

                    return NotFound();
                }
                var res = _mapper.Map<BookDTO>(books);
                _loggerService.LogInfo($"Successfully got  Book with id:{id}");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return InternalError($"{ ex.Message}-{ex.InnerException}");

            }
        }

        /// <summary>
        /// Delete Books
        /// </summary>
        /// <param name="id"></param>
        /// <return>Delete Books </return>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooks(int id = 0)
        {
            try
            {
                _loggerService.LogInfo($"Attempted Get  Books with id :{id}");
                var books = await _bookRepositorycs.GetById(id);
                if (books == null)
                {
                    _loggerService.LogWarn($" Author with id :{id} was not found");

                    return NotFound();
                }
                var isSuccess = await _bookRepositorycs.Delete(books);
                if (!isSuccess)
                {
                    return InternalError($"No Record Found");
                }
                _loggerService.LogInfo($"Book Deleted Successfully with id:{id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ ex.Message}-{ex.InnerException}");

            }
        }
        private ObjectResult InternalError(string message)
        {
            _loggerService.LogError(message);

            return StatusCode(500, "Some thing went wrong please contact to administrator");
        }
    }
}
