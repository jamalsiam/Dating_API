using System;
using System.Collections.Generic;

namespace Api.Dtos
{
    public class PostReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Text { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public int UserId { get; set; }
        public bool Shared { get; set; }
        public bool LikedByAccount { get; set; }
        public int Feeling { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public string Fullname { get; set; }
        public string UserPhotoUrl { get; set; }
        

        
    }
}