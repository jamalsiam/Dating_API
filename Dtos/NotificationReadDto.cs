using System;

namespace Api.Dtos
{
    public class NotificationReadDto
    {
        public int Id { get; set; }
        public int ActionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AppUserId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectFullname { get; set; }
        public string SubjectPhotoUrl { get; set; }
        public int EventId { get; set; }
        public string Description { get; set; }

    }
}