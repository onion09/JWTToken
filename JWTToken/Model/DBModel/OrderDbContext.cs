using Microsoft.EntityFrameworkCore;

namespace JWTToken.Model.DBModel
{
    public class OrderDbContext:DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UsersDetails { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public OrderDbContext(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer(_configuration.GetConnectionString("MyConn1"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders);            
            modelBuilder.Entity<OrderItem>().
                HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems);
        }
    }
}
