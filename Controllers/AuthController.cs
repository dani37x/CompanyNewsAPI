using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyNewsAPI.Controllers
{
    [Tags("AuthController")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _authRepo;

        public AuthController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> RegisterUser(User user)
        {
            return Ok(await _authRepo.RegisterUser(user));
        }

        [HttpPost]
        [Route("Register/Confirmation")]
        public async Task<ActionResult> RegisterUserConfirmation(string key)
        {
            return Ok(await _authRepo.RegisterUserConfirmation(key));
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> LoginUser(Login loginData)
        {
            var token = await _authRepo.LoginUser(loginData);
            if (token.Length > 0)
            {
                return Ok(new { Token = token });
            }
            return BadRequest(new { Message = "Invalid email or password" });
        }
        [HttpPost]
        [Route("NewPassword")]
        public async Task<ActionResult> NewPasswordUser(NewPassword newPassword)
        {
            return BadRequest();
        }
        [HttpPost]
        [Route("NewPassword/Confirmation")]
        public async Task<ActionResult> NewPasswordUserConfirmation(string key)
        {
            return BadRequest();
        }
    }
}
