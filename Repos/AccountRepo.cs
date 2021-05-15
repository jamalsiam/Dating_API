using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Context;
using Api.Dtos;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repos
{
    public class AccountRepo : IAccountRepo
    {
        public DBContext Context { get; }
        public AccountRepo(DBContext context)
        {
            this.Context = context;

        }
        public AppUser Signup(SignupDto signupObj)
        {
            using HMACSHA512 hmac = new HMACSHA512();
            AppUser user = new AppUser()
            {
                UserName = signupObj.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signupObj.Password)),
                PasswordSalt = hmac.Key,
                DateOfBirth= signupObj.Birthdate,
                City = signupObj.City,
                Country= signupObj.Country,
                FirstName = signupObj.FirstName,
                LastName = signupObj.LastName,
                Gender = signupObj.Gender,


            };
            Context.Users.Add(user);

            return user;
        }
        public async Task<AppUser> Signin(SigninDto signinObj)
        {
            AppUser user = await Context.Users.SingleOrDefaultAsync<AppUser>(u => u.UserName == signinObj.Username);
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signinObj.Password));

            for (int i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != user.PasswordHash[i]) return new AppUser { };

            return user;

        }

        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        public AppUser ChangePassword(AppUser user, string password)
        {
            using HMACSHA512 hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.PasswordSalt = hmac.Key;
            return user;
        }
        public async Task<bool> UserExists(string username)
        {
            return await Context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
        }



    }
}