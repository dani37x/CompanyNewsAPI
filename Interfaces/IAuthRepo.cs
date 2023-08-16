using CompanyNewsAPI.Models;

namespace CompanyNewsAPI.Interfaces
{
    public interface IAuthRepo
    {
        public Task<bool> RegisterUser(User user);
        public Task<bool> RegisterConfirmation(string key);
        public Task<string> LoginUser(Login login);
        public Task<bool> NewPasswordUser(User user);
    }
}
