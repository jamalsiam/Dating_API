using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repos
{
    public class UserRepo : IUserRepo
    {
        public readonly DBContext Context;
        public UserRepo(DBContext context)
        {
            this.Context = context;

        }
        public async Task<IEnumerable<AppUser>> AllUsers()
        {
            return await Context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUser(int Id)
        {
            return await Context.Users.FindAsync(Id);
        }

    }
}