using System;
using System.Collections.Generic;
using Api.Entities;

namespace Api.Dtos
{
    public class PostUpdateDto
    {
        public int Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public String Text { get; set; }
        public int Feeling { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public int UserId { get; set; }
    }
}