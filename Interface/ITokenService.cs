using Api.Entities;

namespace Api.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}