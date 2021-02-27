using System;

namespace Api.Entities
{
    public class PostLike
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}