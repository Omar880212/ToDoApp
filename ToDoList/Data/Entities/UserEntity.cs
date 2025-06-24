using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.Data.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        public UserEntity()
        {
            Tasks = new List<TodoTaskEntity>();
            PublicId = Guid.NewGuid().ToString();
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(36)]
        public string PublicId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        [MaxLength(1000)]
        public string Preferences { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public virtual ICollection<TodoTaskEntity> Tasks { get; set; }
    }
}
