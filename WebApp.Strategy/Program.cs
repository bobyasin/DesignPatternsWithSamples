
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Linq;

using WebApp.Strategy.Models;

namespace WebApp.Strategy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            //burada scope oluşturmak için bölümledik

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                // db ye kayıt atmak için bu şekilde scope oluşturduk. kayıt işlemleri bittikten sonra buradaki değişkenler memory den silinecek.
                var appIdentityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                appIdentityDbContext.Database.Migrate();// uygulama ayağa kalktığında uygulanmayan migraion varsa onları uygular. DB yoksa da oluşturur

                if (!userManager.Users.Any())
                {
                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user1",
                        Email = "user1@gmail.com",
                    }, "Password1*").Wait();

                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user2",
                        Email = "user2@gmail.com",
                    }, "Password1*").Wait();

                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user3",
                        Email = "user3@gmail.com",
                    }, "Password1*").Wait();

                    userManager.CreateAsync(new AppUser()
                    {
                        UserName = "user4",
                        Email = "user4@gmail.com",
                    }, "Password1*").Wait();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}