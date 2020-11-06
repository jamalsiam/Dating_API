using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

using System.Text.Json;
using System.Threading.Tasks;
using Api.Context;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.DataSeed
{
    public class Seed
    {
        public static async Task SeedUsers(DBContext context)
        {
            if (await context.Users.AnyAsync()) return;
            var userData = await System.IO.File.ReadAllTextAsync("DataSeed/data.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
                user.PasswordSalt = hmac.Key;
                context.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}