using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser appUser);
    }
}