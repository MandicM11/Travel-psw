﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Travel_psw.Data;
using Travel_psw.Models;

namespace Travel_psw.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _jwtSecret;
        private readonly CartService _cartService;

        public UserService(ApplicationDbContext context, IConfiguration configuration, CartService cartService)
        {
            _context = context;
            _jwtSecret = configuration["Jwt:Key"];
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            if (string.IsNullOrEmpty(_jwtSecret))
            {
                throw new ArgumentNullException("JWT secret is not configured.");
            }

        }

        public async Task<User> AddUserAsync(User user)
        {
            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Proverite da li je _cartService inicijalizovan
            if (_cartService == null)
            {
                throw new InvalidOperationException("CartService is not initialized.");
            }

            await _cartService.CreateCartAsync(user.Id);

            return user;
        }


        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var hashedPassword = HashPassword(password);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);

            return user;
        }

        public string HashPassword(string password)
        {
            // Hash lozinke (u pravoj aplikaciji koristi jaču metodu hashovanja)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}
