using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.Data.Entities
{
    [Table("TodoTasks")]
    public class TodoTaskEntity
    {
        public TodoTaskEntity()
        {
            PublicId = Guid.NewGuid().ToString();
            CreatedDate = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
            Priority = 0;
            Status = "Active";
            IsCompleted = false;
            IsLocked = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string PublicId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModified { get; set; }

        public bool IsLocked { get; set; }

        [MaxLength(100)]
        public string LockedBy { get; set; }

        public DateTime? LockedAt { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }

        [MaxLength(1000)]
        public string Tags { get; set; }
        public int? UserId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
