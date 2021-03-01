using System;

namespace Api.Dtos
{
    public class CommentAddDto
    {
       
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public string CommentPhotoUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
      
    }
}