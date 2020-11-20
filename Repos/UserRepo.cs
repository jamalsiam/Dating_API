using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.Dtos;
using Api.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Repos
{
    public class UserRepo : IUserRepo
    {
        public readonly DBContext Context;
        private readonly IMapper mapper;
        public UserRepo(DBContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.Context = context;

        }



        public async Task<IEnumerable<AppUser>> AllUsers()
        {
            return await Context.Users.ToListAsync();
        }

        public async Task<MemberDto> GetMember(int Id)
        {
            return await Context
            .Users
            .Where(member => member.Id == Id)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<MemberDto>> AllMembers()
        {
            return await Context
              .Users
              .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
              .ToListAsync();
        }

        public async Task<AppUser> GetUser(int Id)
        {
            return await Context.Users.FindAsync(Id);
        }

        public async Task<IEnumerable<MemberDto>> GetMembersByText(string text)
        {
            return await Context
             .Users
             .Where(member =>
              member.UserName.Contains(text) ||
              member.FirstName.Contains(text) ||
              member.LastName.Contains(text))
             .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
             .Take(10)
             .ToListAsync();
        }

     
        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await Context
            .Users
            .Where(member => member.UserName == username)
            
            .SingleOrDefaultAsync();
        }
        public void Update(AppUser member)
        {
           Context.Entry(member).State = EntityState.Modified;
        }

        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }

    }
}