using System;

namespace Api.Entities
{
    public class UserList
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public DateTime CreatedAt { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        
        public string Text { get; set; }
        public DateTime LastActivation { get; set; }
        public string PhotoUrl { get; set; }

    }
}