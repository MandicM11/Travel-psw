namespace Travel_psw.Data
{
    using Microsoft.EntityFrameworkCore;
    using Travel_psw.Models;


    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}

