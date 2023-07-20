﻿using CompanyNewsAPI.Data;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly DataContext _dataContext;

        public UserRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dataContext.Users.ToListAsync();
        }
        public async Task<User> GetSingleUser(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<bool> AddTheUser(User userToAdd)
        {
            _dataContext.Add(userToAdd);
            return await _dataContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateTheUser(User userToUpdate)
        {
            var user = await _dataContext.Users.FindAsync(userToUpdate.Id);

            if (user != null)
            {
                user.FirstName = userToUpdate.FirstName;
                user.LastName = userToUpdate.LastName;
                user.Team = userToUpdate.LastName;
                user.Email = userToUpdate.Email;
                user.Password = userToUpdate.Password;
            }

            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteTheUser(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);

            if (user != null)
            {
                _dataContext.Users.Remove(user);
                await _dataContext.SaveChangesAsync();
            }
           
            if ( await _dataContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

    }
}
