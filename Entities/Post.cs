using System;
using System.Collections.Generic;

namespace Api.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int Feeling { get; set; }
        public String Text { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; } 
        
    }
}