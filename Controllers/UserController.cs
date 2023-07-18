using CompanyNewsAPI.Data;
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
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return Ok(await _dataContext.Users.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetSingleUser(int id)
        {
            var result = _dataContext.Users.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }
            return Ok(await result);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddTheUser(User userToAdd)
        {
            _dataContext.Users.Add(userToAdd);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Users.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<User>> UpdateTheUser(User userToUpdate)
        {
            var result = await _dataContext.Users.FindAsync(userToUpdate.Id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {userToUpdate.Id}");
            }



            result.FirstName = userToUpdate.FirstName;
            result.LastName = userToUpdate.LastName;
            result.Team = userToUpdate.LastName;
            result.Email = userToUpdate.Email;
            result.Password = userToUpdate.Password;

            await _dataContext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> DeleteTheUser(int id)
        {
            var result = await _dataContext.Users.FindAsync(id);

            if (result == null)
            {
                return BadRequest($"0 result for id: {id}");
            }

            _dataContext.Users.Remove(result);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.Users.ToListAsync());
        }
    }
}
