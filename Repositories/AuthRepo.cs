using CompanyNewsAPI.Data;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext _dataContext;

        public AuthRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<bool> Login(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NewPassword(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(User user)
        {
            var existingUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
            {
                return false;
            }

            _dataContext.Add(user);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
