using System;

namespace Api.Entities
{
    public class PostComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public AppUser AppUser { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}