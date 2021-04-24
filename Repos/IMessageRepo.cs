using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;

namespace Api.Repos
{
    public interface IMessageRepo
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<MessageReadDto> GetMessage(int id,int accountId);
        Task<PagedListX<MessageReadDto>> GetMessages(UserParams userParams, int accountId, int userId);
        Task<UserChatDto> GetUserInfo(int Id);
        Task<IEnumerable<UserList>> GetUsersList(int accountId);
        Task<bool> SaveChanges();
    }
}