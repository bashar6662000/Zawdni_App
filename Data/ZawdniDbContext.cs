using Microsoft.EntityFrameworkCore;
using Zawdni.api.Models;
using Zawdni.Models;


namespace Zawdni.api.Data

{
    public class ZawdniDbContext:DbContext
    {
        public ZawdniDbContext(DbContextOptions<ZawdniDbContext> options):base(options)
        {

        }
        public DbSet<Product> products { get; set; }   
        public DbSet<Order > orders { get; set; }
        public DbSet<OrderProduct> orderProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //تعريف مفتاح مركب للجدول الوسيط
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new {op.OrderId, op.ProductId});

            modelBuilder.Entity<OrderProduct>()
           .HasOne(op => op.product)
           .WithMany(p => p.orderProducts)
           .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.order)
                .WithMany(o => o.orderProducts)
                .HasForeignKey(op => op.OrderId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
