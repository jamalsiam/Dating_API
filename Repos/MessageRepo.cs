using System.Linq;
using System.Threading.Tasks;
using Api.Context;
using Api.Entities;
using Api.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Api.Helpers;
using AutoMapper.QueryableExtensions;

namespace Api.Repos
{
    public class MessageRepo : IMessageRepo
    {
        private readonly DBContext Context;
        private readonly IMapper Mapper;
        public MessageRepo(DBContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;

        }

        public void AddMessage(Message message)
        {
            Context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            Context.Messages.Remove(message);
        }

        public async Task<MessageReadDto> GetMessage(int id, int accountId)
        {
            var query = Context.Messages
                .OrderBy(m => m.CreatedAt)
                .Where(a => a.Id == id)
                .Select(m => new MessageReadDto
                {
                    Id = m.Id,
                    AccountId = accountId,
                    RecipientId = m.RecipientId,
                    RecipientFullname = $"{m.Recipient.FirstName} {m.Recipient.LastName}",
                    RecipientPhotoUrl = m.Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                    SenderId = m.SenderId,
                    SenderFullname = $"{m.Sender.FirstName} {m.Sender.LastName}",
                    SenderPhotoUrl = m.Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                    Text = m.Text,
                    DateRead = m.DateRead,
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt,
                    SenderDeleted = m.SenderDeleted,
                    RecipientDeleted = m.RecipientDeleted,
                    Deleted = m.Deleted,
                    Reaction = m.Reaction

                });
            return await query.FirstOrDefaultAsync(m => m.Id == id);

        }

        public async Task<PagedListX<MessageReadDto>> GetMessages(UserParams userParams, int accountId, int userId)
        {

            var query = Context.Messages
                 .OrderBy(m => m.CreatedAt)
                 .Where(a =>
                 (a.SenderId == accountId && a.RecipientId == userId) ||
                 (a.SenderId == userId && a.RecipientId == accountId))
                 .Select(m => new MessageReadDto
                 {
                     Id = m.Id,
                     AccountId = accountId,
                     RecipientId = m.RecipientId,
                     RecipientFullname = $"{m.Recipient.FirstName} {m.Recipient.LastName}",
                     RecipientPhotoUrl = m.Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                     SenderId = m.SenderId,
                     SenderFullname = $"{m.Sender.FirstName} {m.Sender.LastName}",
                     SenderPhotoUrl = m.Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                     Text = m.Text,
                     DateRead = m.DateRead,
                     CreatedAt = m.CreatedAt,
                     UpdatedAt = m.UpdatedAt,
                     SenderDeleted = m.SenderDeleted,
                     RecipientDeleted = m.RecipientDeleted,
                     Deleted = m.Deleted,
                     Reaction = m.Reaction

                 }).OrderByDescending(i => i.Id);

            return await PagedListX<MessageReadDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<UserChatDto> GetUserInfo(int id)
        {
            return await Context
            .Users
            .AsQueryable()
            .Where(usr => usr.Id == id)
            .Select(usr => new UserChatDto
            {
                UserId = usr.Id,
                AccountId = 0,
                Fullname = $"{usr.UserName } {usr.LastName}",
                PhotoUrl = usr.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                TextMessage = "",
                Seen = false,
                MessageCreateAt = DateTime.Now,
                Active = false,
            })
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserList>> GetUsersList(int accountId)
        {


            var userList = await Context.UserList.FromSqlRaw($@"
            SELECT t.SenderId, t.RecipientId, r.MaxTime as createdAt,t.Text, t.id, 

            CASE t.SenderId
                    WHEN {accountId} THEN CONCAT(r.FirstName , ' ' , r.LastName) 
                    
                    ELSE CONCAT(u.FirstName , ' ' , u.LastName)
                end Fullname ,
                
                CASE t.SenderId
                    WHEN {accountId} THEN  r.LastActive 
                    
                    ELSE  u.LastActive
                end lastActivation   ,
    
                CASE t.SenderId
                    WHEN {accountId} THEN    (select p.Url from Dating.photos p where p.ismain = 1 and p.AppUserId = r.id)       
                    ELSE (select p.Url from Dating.photos p where p.ismain = 1 and p.AppUserId = u.id)    
                end PhotoUrl 
                        
            FROM (
                SELECT SenderId, RecipientId, MAX(CreatedAt) as MaxTime
                FROM Dating.Messages
                GROUP BY SenderId, RecipientId
            ) r
            INNER JOIN Dating.Messages t
            ON t.SenderId = r.SenderId AND t.RecipientId = r.RecipientId  AND t.CreatedAt = r.MaxTime
            Inner join Dating.Users u 
            on u.id = t.SenderId
            Inner join Dating.Users r 
            on r.id = t.RecipientId
            where t.RecipientId = {accountId} or t.SenderId = {accountId}")
            .ToListAsync();


 
            return userList;
        }

        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }
}