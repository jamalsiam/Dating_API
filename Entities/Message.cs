using System;

namespace Api.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public AppUser Sender { get; set; }
        public int SenderId { get; set; }
        public AppUser Recipient { get; set; }
        public int RecipientId { get; set; }
        public string Text { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
        public bool Deleted { get; set; } // unsend
        public int Reaction { get; set; }

    }
}