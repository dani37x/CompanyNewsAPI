using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
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

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> RegisterUser(User user)
        {
            return Ok(await _authRepo.RegisterUser(user));
        }

        [HttpPost]
        [Route("Register/Confirmation")]
        public async Task<ActionResult> RegisterConfirmation(string key)
        {
            return Ok(await _authRepo.RegisterConfirmation(key));
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> LoginUser(Login login)
        {
            return Ok(await _authRepo.LoginUser(login));
        }
    }
}
