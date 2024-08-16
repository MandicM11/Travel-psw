using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel_psw.Data;
using Travel_psw.Models;


public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<User> GetUserByIdAsync(int id);
    
}
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    // Dodajte implementacije za druge metode ako su potrebne
}
