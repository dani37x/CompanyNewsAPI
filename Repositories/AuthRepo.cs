using CompanyNewsAPI.Data;
using CompanyNewsAPI.Generators;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using CompanyNewsAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace CompanyNewsAPI.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext _dataContext;
        private readonly EmailService _emailService;
        private readonly string _path = @"keys.json";


        public AuthRepo(DataContext dataContext, EmailService emailService)
        {
            _dataContext = dataContext;
            _emailService = emailService;
        }

        public async Task<bool> RegisterUser(User user)
        {
            var existingUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return false;
            }

            Register registerModel = new Register { Key = KeyGenerator.RandomStr(length: 6), User = user };
            registerModel.User.Email = "daniel.krusinski@nexteer.com";
            var modelData = JsonSerializer.Serialize(registerModel) + ",\n";
            File.AppendAllText(_path, modelData);
            await _emailService.SendEmailAsync(registerModel.User.Email,
                                               "Test",
                                               registerModel.Key);
            return true;
        }

        public async Task<bool> RegisterConfirmation(string key)
        {
            var data = File.ReadAllLines(_path);
            string[] updatedData;

            foreach (var line in data)
            {
                if (line.Contains(key))
                {
                    var modelData = line.Substring(0, line.Length - 1);
                    var registerModel = JsonSerializer.Deserialize<Register>(modelData);
                    var existingUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == registerModel.User.Email);

                    if (DateTime.Now < registerModel.Time && existingUser == null)
                    {
                        User user = registerModel.User;
                        _dataContext.Add(user);
                        await _dataContext.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<bool> LoginUser(Login login)
        {
            var checkUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (checkUser != null)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> NewPasswordUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
