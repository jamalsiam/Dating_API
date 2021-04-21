using System;

namespace Api.Dtos
{
    public class MessageReadDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int SenderId { get; set; }
        public string SenderFullname { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientFullname { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string Text { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
        public bool Deleted { get; set; }
        public int Reaction { get; set; }

    }
}