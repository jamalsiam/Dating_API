using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;

namespace Api.Entities
{
    public class AppUser: IdentityUser<int>
    {
 
        public DateTime DateOfBirth { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public bool Gender { get; set; }
        public string Introduction { get; set; }
        public string Intersts { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserFollow> Followings { get; set; }
        public ICollection<UserFollow> Followers { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Message> MessageSent { get; set; }
        public ICollection<Message> MessageReceived { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

        
 
    }
}