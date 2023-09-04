using CompanyNewsAPI.Models;

namespace CompanyNewsAPI.Interfaces
{
    public interface IAuthRepo
    {
        public Task<bool> RegisterUser(User user);
        public Task<bool> RegisterUserConfirmation(string key);
        public Task<string> LoginUser(Login login);
        public Task<bool> NewPasswordUser(NewPassword newPassword);
        public Task<bool> NewPasswordUserConfirmation(string key);
        public Task<bool> ChangePassword(NewPassword newPassword);
    }
}
