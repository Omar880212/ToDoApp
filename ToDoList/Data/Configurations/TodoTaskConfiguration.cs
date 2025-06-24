using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoListApp.Data.Entities;

namespace TodoListApp.Data.Configurations
{
    public class TodoTaskConfiguration : IEntityTypeConfiguration<TodoTaskEntity>
    {
        public void Configure(EntityTypeBuilder<TodoTaskEntity> builder)
        {
            builder.ToTable("TodoTasks");

            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.PublicId)
                   .IsUnique()
                   .HasName("IX_TodoTasks_PublicId");

            builder.HasIndex(t => t.IsCompleted)
                   .HasName("IX_TodoTasks_IsCompleted");

            builder.HasIndex(t => t.CreatedDate)
                   .HasName("IX_TodoTasks_CreatedDate");

            builder.HasIndex(t => t.Status)
                   .HasName("IX_TodoTasks_Status");

            builder.HasIndex(t => t.DueDate)
                   .HasName("IX_TodoTasks_DueDate");

            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(t => t.Description)
                   .HasMaxLength(2000);

            builder.Property(t => t.PublicId)
                   .IsRequired()
                   .HasMaxLength(36);

            builder.Property(t => t.CreatedBy)
                   .HasMaxLength(100);

            builder.Property(t => t.ModifiedBy)
                   .HasMaxLength(100);

            builder.Property(t => t.LockedBy)
                   .HasMaxLength(100);

            builder.Property(t => t.Status)
                   .HasMaxLength(50)
                   .HasDefaultValue("Active");

            builder.Property(t => t.Category)
                   .HasMaxLength(100);

            builder.Property(t => t.Tags)
                   .HasMaxLength(1000);

            builder.Property(t => t.CreatedDate)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.LastModified)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.RowVersion)
                   .IsRowVersion();
        }
    }
}
