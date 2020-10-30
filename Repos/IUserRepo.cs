using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Repos
{
    public interface IUserRepo
    {
        Task<IEnumerable<AppUser>> AllUsers();
        Task<AppUser> GetUser(int Id);

    }
}