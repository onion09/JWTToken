using JWTToken.Filter;
using JWTToken.Middleware;
using JWTToken.MiddleWare;
using JWTToken.Model.DBModel;
using JWTToken.Services;
using JWTToken.Util;
using Serilog;
using static JWTToken.Services.UseerServices;

namespace JWTToken
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           

            //add service for dependency injection
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<OrderService>();
            builder.Services.AddTransient<IJwtUtils, JWTTokenUtil>();
            //builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddControllers();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            //apply and configure the filter -- Globally
            //builder.Services.AddControllers(options => options.Filters.Add(new MyExceptionFilter())); //typeof(MyExceptionFilter)

            builder.Services.AddDbContext<AuthDBContext>();
            builder.Services.AddDbContext<OrderDbContext>();

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

            app.UseMiddleware<JwtMiddleware>();


            app.UseAuthentication();
            app.UseAuthorization();


            app.UseMiddleware<MyExceptionMiddleware>();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}