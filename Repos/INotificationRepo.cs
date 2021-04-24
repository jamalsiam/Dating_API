using System.Threading.Tasks;
using Api.Dtos;
using Api.Helpers;

namespace Api.Repos
{
    public interface INotificationRepo
    {
        void Add(int userId, int subjectId, int eventId, int actionType);
        void Delete(int id, int accountId);
        Task<PagedListX<NotificationReadDto>> GetAccountNotification(UserParams userParams, int accountId);
        Task<PagedListX<NotificationReadDto>> GetUserNotification(UserParams userParams, int accountId, int userId);
        Task<bool> SaveChanges();
    }
}

