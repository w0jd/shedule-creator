
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
        public DbSet<TaskCategory> TaskCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(p => p.Tasks)
                .WithOne(pc => pc.Users)
                .HasForeignKey(c => c.IdUser);
            modelBuilder.Entity<Category>()
                .HasMany(p =>p.TaskCategories)
                .WithOne(pc => pc.Categories)
                .HasForeignKey(c=>c.IdCategory);
            modelBuilder.Entity<Tasks>()
                .HasMany(p =>p.TaskCategories)
                .WithOne(pc => pc.Tasks)
                .HasForeignKey(c=>c.TaskId);
            modelBuilder.Entity<TaskCategory>()
                .HasKey(pc=>new {pc.IdCategory,pc.TaskId});
        }
 
    }
}