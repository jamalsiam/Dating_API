using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        { }
        
        public DbSet<AppUser> Users { get; set; }
    }
}