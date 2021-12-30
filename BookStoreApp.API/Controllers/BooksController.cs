#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using AutoMapper;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookstoreContext _context;
        private readonly IMapper mapper;

        public BooksController(BookstoreContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReturnDTO>>> GetBooks()
        {
            try
            {
                var bookList = await _context.Books.Include(a => a.Author).ToListAsync();
                List<BookReturnDTO> ret = new List<BookReturnDTO>();

                foreach (var book in bookList)
                {
                    var bookReturnDTO = mapper.Map<BookReturnDTO>(book);
                    bookReturnDTO.Author = mapper.Map<AuthorReturnDto>(book.Author);
                    ret.Add(bookReturnDTO);
                }

                return ret;
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReturnDTO>> GetBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                BookReturnDTO ret = mapper.Map<BookReturnDTO>(book);

                if (book == null)
                {
                    return NotFound();
                }

                return ret;
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO bookDTO)
        {
            try
            {
                if (id != bookDTO.Id)
                {
                    return BadRequest();
                }
                var book = mapper.Map<Book>(bookDTO);
                _context.Entry(book).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
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
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookReturnDTO>> PostBook(BookCreateDTO bookDTO)
        {
            try
            {
                Book book = mapper.Map<Book>(bookDTO);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                var bookReturnDTO = mapper.Map<BookReturnDTO>(book);

                return CreatedAtAction("GetBook", new { id = book.Id }, bookDTO);
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
