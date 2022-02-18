
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using teachersAndStudents.API.Entitys;

namespace teachersAndStudents.API
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }
        public DbSet<Class> Class { set; get; }
        public DbSet<Student> Students { set; get; }
        public DbSet<Teacher> Teachers{ set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
