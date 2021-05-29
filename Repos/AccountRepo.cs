using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Context;
using Api.Dtos;
using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Repos
{
    public class AccountRepo : IAccountRepo
    {

        private readonly UserManager<AppUser> UserManager;
        private readonly SignInManager<AppUser> SignInManager;

        public AccountRepo(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }
        public async Task<AppUser> Signup(SignupDto signupObj)
        {
            using HMACSHA512 hmac = new HMACSHA512();
            AppUser user = new AppUser()
            {
                UserName = signupObj.Username,

                DateOfBirth = signupObj.Birthdate,
                City = signupObj.City,
                Country = signupObj.Country,
                FirstName = signupObj.FirstName,
                LastName = signupObj.LastName,
                Gender = signupObj.Gender,


            };
            var result = await UserManager.CreateAsync(user, signupObj.Password);

            if (!result.Succeeded)
            {
                return null;
            }
            return user;
        }
        public async Task<AppUser> Signin(SigninDto signinObj)
        {
            AppUser user = await UserManager.Users.SingleOrDefaultAsync<AppUser>(u => u.UserName == signinObj.Username);
            var result = await SignInManager.CheckPasswordSignInAsync(user, signinObj.Password, false);
            if (!result.Succeeded)
            {
                return null;
            }
            return user;

        }

        // public async Task<bool> SaveChanges()
        // {
        //     return await UserManager.SaveChangesAsync() > 0;
        // }

        public async Task<IdentityResult> ChangePassword(AppUser user, string oldPassword, string newPassword)
        {
            var result = await UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }
        public async Task<bool> UserExists(string username)
        {
            return await UserManager.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
        }



    }
}