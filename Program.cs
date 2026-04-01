using Microsoft.EntityFrameworkCore;
using Zawdni.api.Data;

namespace Zawdni
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSwaggerGen();

            ///??? ??? ??? ????? 
            builder.Services.AddDbContext<ZawdniDbContext>
                (options =>
            options.UseSqlServer
            (builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            // ? Swagger ?????
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";
            });

            app.MapControllers();

            app.Run();
        }
    }
}
