using System;
using System.Collections.Generic;
using Api.Entities;

namespace Api.Dtos
{
    public class MemberDto
    {

        public int Id { get; set; }
        public string UserName { get; set; }

        public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public bool Gender { get; set; }
        public string Introduction { get; set; }
        public string Intersts { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }

    }
}