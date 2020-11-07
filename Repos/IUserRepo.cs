using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;

namespace Api.Repos
{
    public interface IUserRepo
    {
        Task<IEnumerable<AppUser>> AllUsers();
        Task<AppUser> GetUser(int Id);

        Task<IEnumerable<MemberDto>> AllMembers();
        Task<IEnumerable<MemberDto>> GetMembersByText(string text);
        Task<MemberDto> GetMember(int Id);

    }
}