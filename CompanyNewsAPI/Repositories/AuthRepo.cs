using CompanyNewsAPI.Data;
using CompanyNewsAPI.Generators;
using CompanyNewsAPI.Helpers;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using CompanyNewsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CompanyNewsAPI.Repositories
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext _dataContext;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly string _registratonKeysPath = @"registrationKeys.json";
        private readonly string _newPasswordKeysPath = @"newPasswordKeys.json";

        public AuthRepo(DataContext dataContext, EmailService emailService, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUser(User user)
        {
            var existingUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return false;
            }

            Register registerModel = new Register { Key = await KeysGenerator.RandomStr(path: "registrationKeys.json", length: 6), User = user };
            registerModel.User.Password = SecurityGenerators.ComputeSha256Hash(registerModel.User.Password + registerModel.User.FirstName);
            registerModel.User.Email = "daniel.krusinski@nexteer.com";
            var modelData = "\n" + JsonSerializer.Serialize(registerModel) + ",";
            await FileService.AppendAllTextAsync(_registratonKeysPath, modelData);
            await _emailService.SendEmailAsync(registerModel.User.Email,
                                               "Test",
                                               registerModel.Key);
            return true;
        }
        public async Task<bool> RegisterUserConfirmation(string key)
        {
            var data = File.ReadAllLines(_registratonKeysPath);

            foreach (var line in data)
            {
                if (line.Contains(key))
                {
                    var modelData = line.Substring(0, line.Length - 1);
                    var registerModel = JsonSerializer.Deserialize<Register>(modelData);
                    var existingUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == registerModel.User.Email);

                    if (DateTime.Now < registerModel.Date && existingUser == null)
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
        public async Task<string> LoginUser(Login loginData)
        {
            var getUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email);
            if (getUser != null)
            {
                var password = SecurityGenerators.ComputeSha256Hash(loginData.Password + getUser.FirstName);
                await Console.Out.WriteLineAsync(password);
                if (getUser.Password == password)
                {
                    return SecurityGenerators.GenerateJSONWebToken(loginData, _configuration);
                }
            }
            return "";
        }

        public async Task<bool> ChangePassword(NewPassword newPassword)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == newPassword.Email);
            if (user == null)
            {
                return false;
            }
            user.Password = SecurityGenerators.ComputeSha256Hash(newPassword.Password + user.FirstName);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NewPasswordUser(NewPassword newPassword)
        {
            var checkUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == newPassword.Email);
            if (checkUser == null)
            {
                return false;
            }
            var key = await KeysGenerator.RandomStr(_newPasswordKeysPath, 6);
            newPassword.Password = SecurityGenerators.ComputeSha256Hash(newPassword.Password + checkUser.FirstName);
            newPassword.Key = key;
            newPassword.Date = DateTime.Now.AddMinutes(15);
            //newPassword.Email = "daniel.krusinski@nexteer.com";
            var modelData = "\n" + JsonSerializer.Serialize(newPassword) + ",";
            await FileService.AppendAllTextAsync(_newPasswordKeysPath, modelData);
            await _emailService.SendEmailAsync(newPassword.Email, "New Password key", key);
            return true;
        }
        public async Task<bool> NewPasswordUserConfirmation(string key)
        {
            var data = File.ReadAllLines(_newPasswordKeysPath);

            foreach (var line in data)
            {
                if (line.Contains(key))
                {
                    var modelData = line.Substring(0, line.Length - 1);
                    var newPasswordModel = JsonSerializer.Deserialize<NewPassword>(modelData);
                    var existingUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == newPasswordModel.Email);

                    if (DateTime.Now < newPasswordModel.Date && existingUser != null)
                    {
                        existingUser.Password = newPasswordModel.Password;
                        await _dataContext.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

