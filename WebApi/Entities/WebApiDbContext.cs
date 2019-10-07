using Microsoft.EntityFrameworkCore;

namespace WebApi.Entities {
    public class WebApiDbContext : DbContext {

        public WebApiDbContext (DbContextOptions<WebApiDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(p => new { p.Username });
        }
        public DbSet<User> Users { get; set; }
    }
}