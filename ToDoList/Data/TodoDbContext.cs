using Microsoft.EntityFrameworkCore;
using TodoListApp.Data.Entities;
using TodoListApp.Data.Configurations;
using System.Reflection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace TodoListApp.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }
        
        public TodoDbContext() : base()
        {
        }

        
        public DbSet<TodoTaskEntity> TodoTasks { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new TodoTaskConfiguration().Configure(modelBuilder.Entity<TodoTaskEntity>());

            modelBuilder.Entity<TodoTaskEntity>(entity =>
            {
                entity.HasIndex(e => new { e.Status, e.IsCompleted })
                      .HasName("IX_TodoTasks_Status_IsCompleted");

                entity.HasIndex(e => new { e.CreatedBy, e.CreatedDate })
                      .HasName("IX_TodoTasks_CreatedBy_CreatedDate");
            });

            modelBuilder.Entity<TodoTaskEntity>()
                       .HasOne<UserEntity>()
                       .WithMany(u => u.Tasks)
                       .HasForeignKey("UserId")
                       .OnDelete(DeleteBehavior.SetNull);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TodoListDB;Trusted_Connection=true;MultipleActiveResultSets=true");
            }
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is TodoTaskEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (TodoTaskEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CreatedBy = Environment.UserName;
                }

                entity.LastModified = DateTime.UtcNow;
                entity.ModifiedBy = Environment.UserName;
            }
        }
    }
}
