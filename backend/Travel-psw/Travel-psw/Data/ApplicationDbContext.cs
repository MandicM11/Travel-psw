namespace Travel_psw.Data
{
    using Microsoft.EntityFrameworkCore;
    using Travel_psw.Models;


    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<KeyPoint> KeyPoints { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

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

            modelBuilder.Entity<Cart>()
               .HasOne(c => c.User)
               .WithMany()  // Ili .WithMany(u => u.Carts) ako `User` ima kolekciju `Carts`
               .HasForeignKey(c => c.UserId);

            // Konfiguracija za CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Tour)
                .WithMany()
                .HasForeignKey(ci => ci.TourId);
        }
    }
}

