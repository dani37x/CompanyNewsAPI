using CompanyNewsAPI.Data;
using CompanyNewsAPI.DTO;
using CompanyNewsAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI.Controllers
{
    [Tags("PostController")]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepo _repo;

        public PostController(IPostRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPosts()
        {
            return Ok(await _repo.GetAllPosts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSinglePost(int id)
        {
            return Ok(await _repo.GetSinglePost(id));
        }

        [HttpGet("author/{id}")]
        public async Task<ActionResult> GetAuthorPosts(int id)
        {
            return Ok(await _repo.GetAuthorPosts(id));
        }

        [HttpPost]
        public async Task<ActionResult> AddThePost(PostDto postToAdd)
        {
            return Ok(await _repo.AddThePost(postToAdd));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateThePost(PostDto postToUpdate)
        {
            return Ok(await _repo.UpdateThePost(postToUpdate));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteThePost(int id)
        {
            return Ok(await _repo.DeleteThePost(id));
        }
    }
}
