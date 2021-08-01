using AutoMapper;
using BookStore_App.Contracts;
using BookStore_App.Data;
using BookStore_App.Data.DTO_s;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        public AuthorsController(IMapper mapper, IAuthorRepository authorRepository, ILoggerService loggerService)
        {
            _authorRepository = authorRepository;
            _loggerService = loggerService;
            _mapper = mapper;
        }
        /// <summary>
        /// Get All Authors
        /// </summary>
        /// <return>Return List of Authors</return>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _loggerService.LogInfo("Attempted Get All Authors");
                var authors = await _authorRepository.GetAll();
                var res = _mapper.Map<IList<AuthorDTO>>(authors);
                _loggerService.LogInfo("Successfully got All Authors");

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
        //[Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Create(AuthorCreateDTO dto)
        {
            try
            {
                _loggerService.LogWarn($"Author Submission Attempted");

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
                var authr = _mapper.Map<Author>(dto);
                var isuccess = await _authorRepository.Create(authr);
                if (!isuccess)
                {
                    return InternalError($"Author creation field");
                }
                _loggerService.LogInfo("Author Operation Performed Successfully");
                return Created("Created", new { authr });
            }
            catch (Exception ex)
            {

                return InternalError($"{ ex.Message}-{ex.InnerException}");

            }
        }
        /// <summary>
        /// Update An Authors
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <return>Add Author Authors</return>
        [HttpPut("{id}")]
        //[Authorize(Roles = "Administrator,Customer")]
        public async Task<IActionResult> Update(int id, AuthorUpdateDTO dto)
        {
            _loggerService.LogInfo($"Author with id :{id} Update Attempted");
            try
            {


                if (id < 1 && dto == null && id != dto.Id)
                {
                    _loggerService.LogInfo($"Author with id :{id} Update Attempted");

                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var authr = _mapper.Map<Author>(dto);
                var isuccess = await _authorRepository.Update(authr);
                if (!isuccess)
                {
                    return InternalError($"Author updation field");
                }
                _loggerService.LogInfo("Author Updated Successfully");
                return NoContent();
            }

            catch (Exception ex)
            {

                return InternalError($"{ ex.Message}-{ex.InnerException}");

            }

        }
        /// <summary>
        /// Get Authors
        /// </summary>
        /// <param name="id"></param>
        /// <return>Get By Id Author </return>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id = 0)
        {
            try
            {
                _loggerService.LogInfo($"Attempted Get  Author with id :{id}");
                var authors = await _authorRepository.GetById(id);
                if (authors == null)
                {
                    _loggerService.LogWarn($" Author with id :{id} was not found");

                    return NotFound();
                }
                var res = _mapper.Map<AuthorDTO>(authors);
                _loggerService.LogInfo($"Successfully got  Author with id:{id}");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return InternalError($"{ ex.Message}-{ex.InnerException}");

            }
        }

        /// <summary>
        /// Delete Authors
        /// </summary>
        /// <param name="id"></param>
        /// <return>Delete Author </return>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id = 0)
        {
            try
            {
                _loggerService.LogInfo($"Attempted Get  Author with id :{id}");
                var authors = await _authorRepository.GetById(id);
                if (authors == null)
                {
                    _loggerService.LogWarn($" Author with id :{id} was not found");

                    return NotFound();
                }
                var isSuccess = await _authorRepository.Delete(authors);
                if (!isSuccess)
                {
                    return InternalError($"No Record Found");
                }
                _loggerService.LogInfo($"Author Deleted Successfully with id:{id}");

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
