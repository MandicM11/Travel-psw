namespace Travel_psw.Data
{
    using Microsoft.EntityFrameworkCore;
    using Travel_psw.Models;


    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<KeyPoint> KeyPoints { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.KeyPoints)
                .WithOne()
                .HasForeignKey(kp => kp.TourId);
        }
    }
}

