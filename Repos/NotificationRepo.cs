using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.Dtos;
using Api.Entities;
using Api.Helpers;
using AutoMapper;

namespace Api.Repos
{
    public class NotificationRepo : INotificationRepo
    {
        private readonly DBContext Context;
        private readonly IMapper Mapper;
        public NotificationRepo(DBContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;
        }
        public void Add(int userId, int subjectId, int eventId, int actionType)
        {
            var mappedNotification = Mapper.Map<Notification>(new NotificationAddDto
            {
                ActionType = actionType,
                EventId = eventId,
                SubjectId = subjectId,
                AppUserId = userId
            });

            Context.Notifications.Add(mappedNotification);
            // await SaveChanges();

        }

        public void Delete(int id, int accountId)
        {
            var Notification = Context.Notifications
                        .FirstOrDefault(n => n.AppUserId == accountId && n.Id == id);
            Context.Notifications.Remove(Notification);

        }

        public async Task<PagedListX<NotificationReadDto>> GetAccountNotification(UserParams userParams, int accountId)
        {
            var query = Context.Notifications
                 .OrderBy(m => m.CreatedAt)
                 .Where(a => a.AppUserId == accountId)
                 .Select(n => new NotificationReadDto
                 {
                     Id = n.Id,
                     ActionType = n.ActionType,
                     CreatedAt = n.CreatedAt,
                     AppUserId = n.AppUserId,
                     SubjectId = n.SubjectId,
                     SubjectFullname = $"{n.Subject.FirstName} {n.Subject.LastName}",
                     SubjectPhotoUrl = n.Subject.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                     EventId = n.EventId,
                     Description = ""


                 }).OrderByDescending(i => i.Id);

            return await PagedListX<NotificationReadDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedListX<NotificationReadDto>> GetUserNotification(UserParams userParams, int accountId, int userId)
        {
            var query = Context.Notifications
                 .OrderBy(m => m.CreatedAt)
                 .Where(a => a.AppUserId == accountId && a.SubjectId == userId)
                 .Select(n => new NotificationReadDto
                 {

                     Id = n.Id,
                     ActionType = n.ActionType,
                     CreatedAt = n.CreatedAt,
                     AppUserId = n.AppUserId,
                     SubjectId = n.SubjectId,
                     SubjectFullname = $"{n.Subject.FirstName} {n.Subject.LastName}",
                     SubjectPhotoUrl = n.Subject.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                     EventId = n.EventId,
                     Description = ""
                 }).OrderByDescending(i => i.Id);

            return await PagedListX<NotificationReadDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }
}