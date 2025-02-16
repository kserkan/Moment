using Microsoft.EntityFrameworkCore;
using Moment.Models;

namespace Moment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Follower -> User ilişkisi
            modelBuilder.Entity<Follower>()
                .HasOne(f => f.Followers) // Follower tablosundaki Follower özelliği
                .WithMany(u => u.Following) // User'ın takip ettiği kişiler
                .HasForeignKey(f => f.FollowerId) // FollowerId User tablosuna referans eder
                .OnDelete(DeleteBehavior.Cascade);

            // Followed -> User ilişkisi
            modelBuilder.Entity<Follower>()
                .HasOne(f => f.Followed) // Follower tablosundaki Followed özelliği
                .WithMany(u => u.Followers) // User'ın takipçileri
                .HasForeignKey(f => f.FollowedId) // FollowedId User tablosuna referans eder
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
