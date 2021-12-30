#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookstoreContext _context;
        private readonly IMapper mapper;

        public AuthorsController(BookstoreContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReturnDto>>> GetAuthors()
        {
            try
            {
                var authorList = await _context.Authors.ToListAsync();
                List<AuthorReturnDto> authorListReturn = new List<AuthorReturnDto>();
                foreach (var author in authorList)
                {
                    var authorReturn = mapper.Map<AuthorReturnDto>(author);
                    authorListReturn.Add(authorReturn);
                }

                //var authorList = await _context.Authors.ToListAsync();
                //List<AuthorReturnDto> authorListReturn = mapper.Map<IEnumerable<AuthorReturnDto>>(authorList);

                return authorListReturn;
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReturnDto>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                var autherReturn = mapper.Map<AuthorReturnDto>(author);

                if (author == null)
                {
                    return NotFound();
                }

                return autherReturn;
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            try
            {
                if (id != authorDto.Id)
                {
                    return BadRequest();
                }
                var author = mapper.Map<AuthorUpdateDto>(authorDto);
                _context.Entry(author).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                var author = mapper.Map<Author>(authorDto);
                var authorReturn = mapper.Map<AuthorReturnDto>(author);
                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = authorReturn.Id }, authorReturn);
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                //log here
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
