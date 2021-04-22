
using Api.Context;
using Api.Helpers;
using Api.Interface;
using Api.Repos;
using Api.Service;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IFollowRepo, FollowRepo>();
            services.AddScoped<IPostRepo, PostRepo>();
            services.AddScoped<INotificationRepo, NotificationRepo>();
            services.AddScoped<IPostCommentRepo, PostCommentRepo>();
            services.AddScoped<IPostLikeRepo, PostLikeRepo>();
            services.AddScoped<IMessageRepo, MessageRepo>();

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DBContext>(opt => opt.UseMySQL(config.GetConnectionString("dbConnection")));

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IAccountRepo, AccountRepo>();

            services.AddCors();
            return services;
        }
    }
}