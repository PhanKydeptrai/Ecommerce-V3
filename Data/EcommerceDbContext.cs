using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(){}
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options){}

        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<Recomment> Recomments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(x => x.UserId);
                entity.Property(a => a.FirstName).IsRequired().HasColumnType("nvarchar(30)");
                entity.Property(a => a.LastName).IsRequired().HasColumnType("nvarchar(30)");
                entity.Property(a => a.Email).IsRequired().HasColumnType("varchar(50)");
                entity.Property(a => a.Password).IsRequired().HasColumnType("varchar(max)");
                entity.Property(a => a.Status).IsRequired().HasColumnType("varchar(10)");
                entity.Property(a => a.Role).IsRequired().HasColumnType("varchar(10)");
                entity.Property(a => a.Gender).IsRequired().HasColumnType("nvarchar(15)");
                entity.Property(a => a.Address).IsRequired(false).HasColumnType("nvarchar(150)");
                entity.Property(a => a.Image).IsRequired(false).HasColumnType("varchar(max)");
                entity.Property(a => a.DateOfBirth).IsRequired().HasColumnType("datetime");
                entity.Property(a => a.PhoneNumber).IsRequired().HasColumnType("varchar(10)");
                //Khóa ngoại cho User và Order
                entity.HasMany(a => a.Orders).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(a => a.OrderId);
                entity.Property(a => a.UserId).IsRequired();
                entity.Property(a => a.CreatedOfDate).IsRequired().HasColumnType("datetime");
                entity.Property(a => a.Status).IsRequired().HasColumnType("varchar(10)");
                entity.Property(a => a.UnitPrice).IsRequired().HasColumnType("decimal(18, 2)");
                //Khóa ngoại cho Order và OrderDetail
                entity.HasMany(a => a.OrderDetails).WithOne(a => a.Order).HasForeignKey(a => a.OrderId);
                //Khóa ngoại cho Order và Recommnent
                entity.HasOne(a => a.Recomment).WithOne(a => a.Order).HasForeignKey<Recomment>(a => a.OrderId);
            });

            modelBuilder.Entity<OrderDetail>(entity => {
                entity.HasKey(a => a.OrderDetailId);
                entity.Property(a => a.OrderId).IsRequired();
                entity.Property(a => a.ItemId).IsRequired();
                entity.Property(a => a.Quanlity).IsRequired().HasColumnType("int");
                entity.Property(a => a.Price).IsRequired().HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Recomment>(entity =>
            {
                entity.HasKey(a => a.CommentId);
                //entity.Property(a => a.CommentId).IsRequired().HasColumnType("varchar(5)");
                entity.Property(a => a.Comment).IsRequired().HasColumnType("varchar(255)");
                entity.Property(a => a.Image).IsRequired(false).HasColumnType("varchar(max)");
                entity.Property(a => a.OrderId).IsRequired();

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(a => a.ProductId);
                entity.Property(a => a.ProductName).IsRequired().HasColumnType("nvarchar(30)");
                entity.Property(a => a.Description).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(a => a.Quanlity).IsRequired().HasColumnType("int");
                entity.Property(a => a.Size).IsRequired().HasColumnType("nvarchar(10)");
                entity.Property(a => a.Price).IsRequired().HasColumnType("decimal(18, 2)");
                entity.Property(a => a.Image).IsRequired(false).HasColumnType("varchar(max)");
                entity.Property(a => a.CategoryId).IsRequired();
                entity.Property(a => a.Unit).IsRequired().HasColumnType("varchar(10)");
                entity.Property(a => a.ItemId).IsRequired();
                //Khóa ngoại cho Product và ComboDetail
                entity.HasMany(a => a.ComboDetails).WithOne(a => a.Product).HasForeignKey(a => a.ProductId).OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(a => a.ItemId);
                entity.Property(a => a.ItemId).IsRequired();
                entity.Property(a => a.ItemType).IsRequired().HasColumnType("varchar(10)");
                entity.Property(a => a.Status).IsRequired().HasColumnType("varchar(10)");
                entity.HasOne(a => a.Product).WithOne(a => a.Item).HasForeignKey<Product>(a => a.ItemId);
                entity.HasOne(a => a.Combo).WithOne(a => a.Item).HasForeignKey<Combo>(a => a.ItemId);
                entity.HasMany(a => a.OrderDetail).WithOne(a => a.Item).HasForeignKey(a => a.ItemId);
            });

            modelBuilder.Entity<Combo>(entity =>
            {
                entity.HasKey(a => a.ComboId);
                entity.Property(a => a.ComboName).IsRequired().HasColumnType("nvarchar(30)");
                entity.Property(a => a.Price).IsRequired().HasColumnType("decimal(18, 2)");
                entity.Property(a => a.Description).IsRequired().HasColumnType("nvarchar(max)");
                entity.Property(a => a.Image).IsRequired(false).HasColumnType("varchar(max)");
                entity.Property(a => a.ItemId).IsRequired();
                
                //Khóa ngoại cho Combo và ComboDetail
                entity.HasMany(a => a.ComboDetails).WithOne(a => a.Combo).HasForeignKey(a => a.ComboId);

            });

            modelBuilder.Entity<ComboDetail>(entity =>
            {
                entity.HasKey(a => a.ComboDetailId);
                entity.Property(a => a.ComboId).IsRequired();
                entity.Property(a => a.ProductId).IsRequired();
                entity.Property(a => a.Quanlity).IsRequired().HasColumnType("int");
            });
            
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(a => a.CategoryId);
                entity.Property(a => a.CategoryName).IsRequired().HasColumnType("nvarchar(30)");
                entity.Property(a => a.Image).IsRequired(false).HasColumnType("varchar(max)");
                entity.Property(a => a.Status).IsRequired().HasColumnType("varchar(10)");
                entity.HasMany(a => a.Products).WithOne(a => a.Category).HasForeignKey(c => c.CategoryId);
            });
        }
    }
}
