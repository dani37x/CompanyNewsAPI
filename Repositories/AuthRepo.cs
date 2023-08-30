using CompanyNewsAPI.Data;
using CompanyNewsAPI.Generators;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using CompanyNewsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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

            Register registerModel = new Register { Key = await KeyGenerator.RandomStr(path: "registrationKeys.json", length: 6), User = user };
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
            string[] updatedData;

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
            var checkUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == loginData.Password);

            if (checkUser != null)
            {
                return GenerateJSONWebToken(loginData);
            }
            return "";
        }

        public async Task<bool> NewPasswordUser(NewPassword newPassword)
        {
            throw new NotImplementedException();

        }
        public async Task<bool> NewPasswordUserConfirmation(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(NewPassword newPassword)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == newPassword.Email);
            if (user != null)
            {
                user.Password = newPassword.Password;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public string GenerateJSONWebToken(Login loginData)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginData.Email),
                new Claim(ClaimTypes.Role, "user")
             };

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

