using System;
using System.Collections.Generic;
using Api.Entities;

namespace Api.Dtos
{
    public class PostAddDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt = DateTime.Now;
        public DateTime UpdatedAt = DateTime.Now;
        public String Text { get; set; }
        public int Feeling { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public int UserId { get; set; }
    }
}