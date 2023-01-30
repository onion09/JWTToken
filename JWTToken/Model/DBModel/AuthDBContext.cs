using Microsoft.EntityFrameworkCore;

namespace JWTToken.Model.DBModel
{
    public class AuthDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public AuthDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer(_configuration.GetConnectionString("MyConn"));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new {up.UserId, up.PermissionId });
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.PermissionId);
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);
        }

    }
}
