using Microsoft.EntityFrameworkCore;

namespace Mmicovic.RPSSL.Service.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base() { }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        { }

        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set UserName as primary key
            modelBuilder.Entity<User>().HasKey(x => x.UserName);
            base.OnModelCreating(modelBuilder);
        }
    }
}
