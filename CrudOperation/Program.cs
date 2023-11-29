using CrudOperation.Models;
using CrudOperation.Reprositry;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Threading.Tasks;

namespace CrudOperation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IMovieReprositry, MovieReprositry>();
            builder.Services.AddScoped<IGenreReprositry,GenreReprositry>();
            builder.Services.AddScoped<IAccountReprositry, AccountReprositry>();
            builder.Services.AddScoped<AppUser, AppUser>();
            builder.Services.AddScoped<IReprositryReports, ReprositryReports>();
            //Custom autho
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //               .AddCookie();
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopCenter,
                PreventDuplicates = true,
                CloseButton = true
                
            });
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