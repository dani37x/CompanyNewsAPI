using CompanyNewsAPI.Data;
using CompanyNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CompanyAuthorsAPI.Controllers
{
    [Tags("AuthorController")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorContoller : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AuthorContoller(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAllAuthors()
        {
            return Ok(await _dataContext.Authors.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetSingleAuthor(int id)
        {
            var result = _dataContext.Authors.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }
            return Ok(await result);
        }

        [HttpPost]
        public async Task<ActionResult<List<Author>>> AddTheAuthor(Author authorToAdd)
        {
            _dataContext.Authors.Add(authorToAdd);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Authors.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<Author>> UpdateTheAuthor(Author authorToUpdate)
        {
            var result = await _dataContext.Authors.FindAsync(authorToUpdate.Id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {authorToUpdate.Id}");
            }

            result.FirstName = authorToUpdate.FirstName;
            result.LastName = authorToUpdate.LastName;
            result.Team = authorToUpdate.LastName;

            await _dataContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Author>>> DeleteTheAuthor(int id)
        {
            var result = await _dataContext.Authors.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }

            _dataContext.Authors.Remove(result);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Authors.ToListAsync());
        }
    }
}
