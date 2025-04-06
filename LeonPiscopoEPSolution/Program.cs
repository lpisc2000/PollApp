using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LeonPiscopoEPSolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PollDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PollDb")));

            builder.Services.AddScoped<IPollRepository,PollRepository>();
            // To use file repository instead, swap the above with:
            // builder.Services.AddSingleton<IPollRepository>(new PollFileRepository("polls.json"))

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(); // enabling session for login tracking

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession(); 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
