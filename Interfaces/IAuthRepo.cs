using CompanyNewsAPI.Models;

namespace CompanyNewsAPI.Interfaces
{
    public interface IAuthRepo
    {
        public Task<bool> Register(User user);
        public Task<bool> Login(User user);
        public Task<bool> NewPassword(User user);
    }
}
