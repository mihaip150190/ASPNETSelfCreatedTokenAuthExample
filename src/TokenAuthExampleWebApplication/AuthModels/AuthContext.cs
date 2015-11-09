using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace TokenAuthExampleWebApplication.AuthModels
{
    public partial class AuthContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer(@"data source=salia-sql;initial catalog=TestAuth;user id=sa;MultipleActiveResultSets=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleID).ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Username).IsRequired();

                entity.HasOne(d => d.Role).WithMany(p => p.User).ForeignKey(d => d.RoleID);
            });
        }

        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}