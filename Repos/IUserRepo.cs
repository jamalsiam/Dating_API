using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;

namespace Api.Repos
{
    public interface IUserRepo
    {
        Task<IEnumerable<AppUser>> AllUsers();
        Task<AppUser> GetUser(int Id);

        Task<PagedList<MemberDto>> AllMembers(UserParams userParams);
        Task<IEnumerable<MemberDto>> GetMembersByText(string text);
        Task<MemberDto> GetMember(int Id);
       void Update(AppUser member);
        Task<AppUser> GetUserByUsername(string username);
        Task<bool> SaveChanges();
       
    }
}