using System;
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
    public class PostRepo : IPostRepo
    {

        private readonly DBContext Context;
        private readonly IMapper Mapper;

        public PostRepo(DBContext context, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Context = context;

        }
        public void AddPost(Post post)
        {
            Context.Posts.Add(post);
        }

        public async void DeletePost(int userId, int postId)
        {
            var post = await Context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.AppUserId == userId);
            Context.Posts.Remove(post);
        }

        public async Task<PostReadDto> GetPost(int postId)
        {
            var post = Context.Posts.Where(p => p.Id == postId);
            var p = await post.FirstOrDefaultAsync();
            var phs = await Context.Photos.Where(ph => ph.PostId == postId).ToListAsync();

            return await post.Select(f => f.AppUser)
            .Select(u => new PostReadDto
            {
                Id = p.Id,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Text = p.Text,
                UserId = u.Id,
                Feeling = p.Feeling,
                Fullname = $"{u.FirstName } {u.LastName}",
                UserPhotoUrl = u.Photos.FirstOrDefault(p => p.IsMain).Url,
                Photos = Mapper.Map<ICollection<PhotoDto>>(phs),
                Shared = false,
                LikedByAccount = false,
                LikesCount = 0,
                CommentsCount = 0,
            })
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PostReadDto>> GetPosts(int userId)
        {

            var posts = Context.Posts.Where(p => p.AppUserId == userId).AsQueryable();

            var postsList = posts.Select(p => new PostReadDto
            {
                Id = p.Id,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Text = p.Text,
                UserId = p.AppUser.Id,
                Feeling = p.Feeling,
                Fullname = $"{p.AppUser.FirstName } {p.AppUser.LastName}",
                UserPhotoUrl = p.AppUser.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                Photos = Mapper.Map<ICollection<PhotoDto>>(p.Photos.Where(ph => ph.PostId == p.Id).ToList()),
                Shared = false,
                LikedByAccount = false,
                LikesCount = 0,
                CommentsCount = 0,
            });
            return await postsList.OrderByDescending(p => p.Id).ToListAsync();

        }



        public async Task<IEnumerable<PostReadDto>> GetPosts(int userId, bool category)
        {
            if (!category) return await GetPosts(userId);
            var postdList = new List<PostReadDto>();
            var followingsPosts = from post in Context.Posts
                                  join userfollow in Context.Follow on post.AppUserId equals userfollow.FollowerId
                                  join createBy in Context.Users on userfollow.FollowerId equals createBy.Id
                                  where userfollow.FollowingId == userId
                                  select new PostReadDto()
                                  {
                                      Id = post.Id,
                                      Text = post.Text,
                                      Fullname = $"{createBy.FirstName } {createBy.LastName}",
                                      CreatedAt = post.UpdatedAt,
                                      UpdatedAt = post.UpdatedAt,
                                      UserId = createBy.Id,
                                      UserPhotoUrl = createBy.Photos.FirstOrDefault(ph => ph.IsMain).Url,
                                      Photos = Mapper.Map<ICollection<PhotoDto>>(createBy.Photos.Where(ph => ph.PostId == post.Id).ToList()),
                                      Feeling = post.Feeling,
                                      Shared = false,
                                      LikedByAccount = false,
                                      LikesCount = 0,
                                      CommentsCount = 0,
                                  };

            postdList.AddRange(followingsPosts);
            var accountPosts = await GetPosts(userId);
            postdList.AddRange(accountPosts);

            return postdList.OrderBy(p => p.CreatedAt).Reverse().ToList();

        }

        public async Task<bool> SaveChanges()
        {
            return await Context.SaveChangesAsync() > 0;
        }


        public void UpdatePost(PostUpdateDto post)
        {
            var mappedPost = Mapper.Map<Post>(post);
            var currentPost = Context.Posts.Find(post.Id);
            currentPost.Text = mappedPost.Text;
            currentPost.UpdatedAt = DateTime.Now;
            currentPost.Feeling = mappedPost.Feeling;

            var photos =  mappedPost.Photos.Select(ph => ph.Id);
            if (photos.Any())
            {
                 Context.RemoveRange(Context.Photos.Where(ph => photos.Contains(ph.Id)));
            }
           

        }


    }
}