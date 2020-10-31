using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class SignupDto
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}