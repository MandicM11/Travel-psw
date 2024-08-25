using Travel_psw.Data;
using Travel_psw.Models;
using Travel_psw.Services;
using Microsoft.EntityFrameworkCore;

public class AdminService
{
    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;
    private readonly EmailService _emailService;

    public AdminService(ApplicationDbContext context, UserService userService, EmailService emailService)
    {
        _context = context;
        _userService = userService;
        _emailService = emailService;
    }

    // Identify malicious users
    public async Task UpdateUserStatusAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Check if the tourist is malicious
        if (user.InvalidReportCount >= 10)
        {
            user.IsMalicious = true;
        }

        // Check if the author is malicious
        if (user.UnresolvedReviewCount >= 10)
        {
            user.IsMalicious = true;
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    // Block a user
    public async Task BlockUserAsync(int userId)
    {
        // Ažuriraj status korisnika pre blokiranja
        await UpdateUserStatusAsync(userId);

        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (user.IsMalicious)
        {
            user.IsBlocked = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Send email notification to the user
            string subject = "Your account has been blocked";
            string body = "Your account has been blocked due to malicious behavior. Please contact support if you believe this is a mistake.";
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
    }

    // Unblock a user
    public async Task UnblockUserAsync(int userId)
    {
        // Ažuriraj status korisnika pre odblokiranja
        await UpdateUserStatusAsync(userId);

        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new Exception("User not found.");
        }


        user.IsBlocked = false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        // Send email notification to the user
        string subject = "Your account has been unblocked";
        string body = "Your account has been reactivated. You can now log in to the system.";
        await _emailService.SendEmailAsync(user.Email, subject, body);
    }


    // Get a list of malicious users
    public async Task<List<User>> GetMaliciousUsersAsync()
    {
        // Ažuriraj status svih korisnika pre nego što ih zatražiš
        var users = await _context.Users.ToListAsync();
        foreach (var user in users)
        {
            await UpdateUserStatusAsync(user.Id);
        }

        return await _context.Users.Where(u => u.IsMalicious).ToListAsync();
    }

    // Prevent blocked users from logging in
    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _userService.AuthenticateUserAsync(username, password);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (user.IsBlocked)
        {
            throw new UnauthorizedAccessException("Your account has been blocked.");
        }

        return user;
    }
}

