using CompanyNewsAPI.Data;
using CompanyNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI.Controllers
{
    [Tags("PostController")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PostController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<Post>> GetAllPosts()
        {
            return Ok(await _dataContext.Posts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetSinglePosts(int id)
        {
            var result = _dataContext.Posts.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }
            return Ok(await result);
        }

        [HttpGet("author/{id}")]
        public async Task<ActionResult<List<Post>>> GetAuthorPosts(int id)
        {
            var author = await _dataContext.Users.FindAsync(id);

            if (author == null)
            {
                return BadRequest($"0 result for id: {id}");
            }

            return Ok(await _dataContext.Posts.Where(user => user.UserId == id).ToListAsync());
        }


        [HttpPost]
        public async Task<ActionResult<Post>> AddThePosts(PostDto request)
        {
            var author = await _dataContext.Users.FindAsync(request.UserId);
            if (author == null)
            {
                return NotFound();
            }
            var postToAdd = new Post
            {
                Title = request.Title,
                Description = request.Description,
                Date = DateTime.Now,
                User = author
            };
            await _dataContext.Posts.AddAsync(postToAdd);
            await _dataContext.SaveChangesAsync();

            return Ok("Object has been added");
        }

        [HttpPut]
        public async Task<ActionResult<Post>> UpdateThePosts(PostDto request)
        {
            var post = await _dataContext.Posts.FindAsync(request.Id);

            if (post == null)
            {
                return BadRequest($"0 result for id: {request.Id}");
            }
            var user = await _dataContext.Users.FindAsync(request.UserId);

            post.Id = request.Id;
            post.Title = request.Title;
            post.Description = request.Description;
            post.UserId = request.UserId;
            post.User = user;

            await _dataContext.SaveChangesAsync();
            return Ok("Changes have been updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Post>>> DeleteThePosts(int id)
        {
            var result = await _dataContext.Posts.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }

            _dataContext.Posts.Remove(result);
            await _dataContext.SaveChangesAsync();
            return Ok($"Row {id} has been deleted");
        }
    }
}
