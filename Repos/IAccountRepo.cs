using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;

namespace Api.Repos
{
    public interface IAccountRepo
    {
        AppUser Signup(SignupDto signupObj);
        Task<AppUser> Signin(SigninDto signinObj);

        Task<bool> UserExists(string username);
        Task<bool> SaveChanges();

    }
}