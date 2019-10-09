using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace DataAccess {
    public class WebApiDbContext : DbContext {

        public WebApiDbContext (DbContextOptions<WebApiDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(p => new { p.Username }).IsUnique(true);
        }
        public DbSet<User> Users { get; set; }
    }
}