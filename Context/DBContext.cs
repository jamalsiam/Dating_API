using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Context
{
    public class DBContext : IdentityDbContext<AppUser,AppRole,int,
    IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,
    IdentityRoleClaim<int>,IdentityUserToken<int>
    >
    {
        public DBContext(DbContextOptions options) : base(options)
        { }

        public DbSet<UserFollow> Follow { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserList> UserList { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<AppUser>()
            .HasMany(u => u.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(u => u.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .IsRequired();
            
            builder.Entity<UserFollow>()
            .HasKey(key => new { key.FollowingId, key.FollowerId });

            builder.Entity<UserFollow>()
            .HasOne(u => u.Following)
            .WithMany(u => u.Followers)
            .HasForeignKey(u => u.FollowingId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollow>()
            .HasOne(u => u.Follower)
            .WithMany(u => u.Followings)
            .HasForeignKey(u => u.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessageReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
           .HasOne(u => u.Sender)
           .WithMany(m => m.MessageSent)
           .OnDelete(DeleteBehavior.Restrict);



        }
    }
}