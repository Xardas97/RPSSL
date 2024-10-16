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
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserName);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
