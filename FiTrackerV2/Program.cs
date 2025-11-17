using FiTrackerV2.BLL;
using FiTrackerV2.DAL;
using FiTrackerV2.Domain.Interfaces;

namespace FiTrackerV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string connectionString = "Server=mssqlstud.fhict.local;Database=dbi297707_fitracker;User Id=dbi297707_fitracker;Password=Password123;TrustServerCertificate=True;";
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IExerciseRepository>(_ => new ExerciseRepository(connectionString));
            builder.Services.AddScoped<ExerciseService>();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
