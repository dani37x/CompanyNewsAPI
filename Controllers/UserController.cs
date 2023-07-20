using CompanyNewsAPI.Data;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI.Controllers
{
    [Tags("UserController")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;

        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            return Ok(await _repo.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSingleUser(int id)
        {
            return Ok(await _repo.GetSingleUser(id));
        }

        [HttpPost]
        public async Task<ActionResult> AddTheUser(User userToAdd)
        {
            return Ok(await _repo.AddTheUser(userToAdd));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTheUser(User userToUpdate)
        {
            return Ok(await _repo.UpdateTheUser(userToUpdate));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTheUser(int id)
        {
            return Ok(await _repo.DeleteTheUser(id));
        }
    }
}
