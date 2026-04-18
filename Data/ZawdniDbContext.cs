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
        public DbSet<User> Users { get; set; }
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
            // --- إضافة بيانات الـ Seeding هنا ---

            // 1. إضافة مستخدمين تجريبيين
            modelBuilder.Entity<User>().HasData
                (
                new User { Id = 1, Name = "bashar", Password = "password123",Email="bashar66629@gmail.com",PhoneNumber="0996400131",Role="Admin",ClinicAddress="inshaat_brazeel",ClinicName="testclinic"}
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
