using CompanyNewsAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexteerNewsAPI.Models;

namespace CompanyNewsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyNewsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CompanyNewsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyNews>>> GetAllNews()
        {
            return Ok(await _dataContext.News.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<CompanyNews>>> GetSingleNews(int id)
        {
            var result = _dataContext.News.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<CompanyNews>>> AddTheNews(CompanyNews newsToAdd)
        {
            _dataContext.News.Add(newsToAdd);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.News.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<CompanyNews>>> UpdateTheNews(CompanyNews newsToUpdate)
        {
            var result = await _dataContext.News.FindAsync(newsToUpdate.Id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {newsToUpdate.Id}");
            }

            result.Id = newsToUpdate.Id;
            result.Title = newsToUpdate.Title;
            result.Description = newsToUpdate.Description;
            result.Author = newsToUpdate.Author;
            result.Date = newsToUpdate.Date;

            await _dataContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<CompanyNews>>> DeleteTheNews(int id)
        {
            var result = await _dataContext.News.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }

            _dataContext.News.Remove(result);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.News.ToListAsync());
        }
    }
}
