using System;

namespace Api.Dtos
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string UserPhotoUrl { get; set; }
        public string Text { get; set; }
        public string CommentPhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsAuthor { get; set; }
        public bool IsAccountComment { get; set; }
    }
}