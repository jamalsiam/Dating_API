using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Repos
{
    public interface IAccountRepo
    {
        Task<AppUser> Signup(SignupDto signupObj);
        Task<AppUser> Signin(SigninDto signinObj);

        Task<bool> UserExists(string username);
       // Task<bool> SaveChanges();
        Task<IdentityResult> ChangePassword(AppUser user, string oldPassword, string newPassword);
 
    }
}