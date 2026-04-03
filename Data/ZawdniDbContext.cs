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
    }
}
