using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class SignupDto
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime Birthdate { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }



 
 
    }
}