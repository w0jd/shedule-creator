
using Microsoft.EntityFrameworkCore;
using shedule_app.Models;

namespace shedule_app.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {

        }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(p => p.Tasks)
                .WithOne(pc => pc.Users)
                .HasForeignKey(c => c.IdUser);
            modelBuilder.Entity<Category>()
                .HasMany(p =>p.Tasks)
                .WithOne(pc => pc.Categories)
                .HasForeignKey(c=>c.IdCategory);
        }
 
    }
}