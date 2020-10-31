using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class SigninDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}