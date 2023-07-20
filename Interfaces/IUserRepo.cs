using CompanyNewsAPI.Models;

namespace CompanyNewsAPI.Interfaces
{
    public interface IUserRepo
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> GetSingleUser(int id);
        public Task<bool> AddTheUser(User userToAdd);
        public Task<bool> UpdateTheUser(User UpdateTheUser);
        public Task<bool> DeleteTheUser(int id);

    }
}
