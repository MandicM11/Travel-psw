using Microsoft.EntityFrameworkCore;
using Travel_psw.Models;

namespace Travel_psw.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<KeyPoint> KeyPoints { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Purchase> Purchases { get; set; }  
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemEvent> ProblemEvents { get; set; }

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
                .HasForeignKey(kp => kp.TourId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tour>()
                .HasOne(t => t.Author)
                .WithMany(u => u.Tours)
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Sales)
                .WithOne()
                .HasForeignKey(s => s.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .Property(t => t.Status)
                .HasConversion(
                    v => (int)v,
                    v => (TourStatus)v
                );

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Tour)
                .WithMany()
                .HasForeignKey(ci => ci.TourId);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Tour)
                .WithMany(t => t.Sales)
                .HasForeignKey(s => s.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sales)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
            .HasOne(p => p.Tour)
            .WithMany(t => t.Purchases)
            .HasForeignKey(p => p.TourId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Problem>()
                .HasOne(p => p.Tour)           
                .WithMany(t => t.Problems)     
                .HasForeignKey(p => p.TourId)  
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Problem>().ToTable("Problems");
            modelBuilder.Entity<ProblemEvent>().ToTable("ProblemEvents");

            modelBuilder.Entity<ProblemEvent>()
            .HasDiscriminator<string>("EventType")
            .HasValue<ProblemReportedEvent>("ProblemReportedEvent")
            .HasValue<ProblemResolvedEvent>("ProblemResolvedEvent")
            .HasValue<ProblemSentForReviewEvent>("ProblemSentForReviewEvent")
            .HasValue<ProblemRejectedEvent>("ProblemRejectedEvent");

        }
    }
}
