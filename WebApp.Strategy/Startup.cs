using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Linq;

using WebApp.Strategy.Models;
using WebApp.Strategy.StrategyRepository;

namespace WebApp.Strategy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHttpContextAccessor(); // bu servis üzerinden herhangi bir class constructor ı içinde HttpContext e erişebiliriz ( DI ile eklenebilir)

            services.AddScoped<IProductRepository>(sp =>
            {
                var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;

                var claim = httpContext.User.Claims.FirstOrDefault(f => f.Type == Settings.DatabaseClaimType);

                var dbContext = sp.GetRequiredService<AppIdentityDbContext>();

                if (claim == null)
                {
                    return new SqlServerRepository(dbContext);
                }
                else
                {
                    var dbType = (DatabaseType)int.Parse(claim.Value);

                    return dbType switch
                    {
                        DatabaseType.SqlServer => new SqlServerRepository(dbContext),
                        DatabaseType.MongoDb => new MongoDbRepository(Configuration)
                    };
                }
            });

            services.AddSession(so => { });
            services.AddAntiforgery(ao => { });

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnStr"));
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}