using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        public int Id { get; set; }
        public int ActionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public AppUser Subject { get; set; }
        public int SubjectId { get; set; }
        public int EventId { get; set; }
    }
}