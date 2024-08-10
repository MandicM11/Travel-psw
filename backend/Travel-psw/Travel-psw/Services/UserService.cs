using Microsoft.EntityFrameworkCore;
using System;
using Travel_psw.Data;
using Travel_psw.Models;

namespace Travel_psw.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            user.Password = HashPassword(user.Password); 

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public string HashPassword(string password)
        {
            // Hash lozinke (u pravoj aplikaciji koristi jaču metodu hashovanja)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
