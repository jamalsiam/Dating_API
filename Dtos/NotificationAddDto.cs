using System;

namespace Api.Dtos
{
    public class NotificationAddDto
    {
        public int Id { get; set; }
        public int ActionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int AppUserId { get; set; }
        public int SubjectId { get; set; }
        public int EventId { get; set; }
    }
}