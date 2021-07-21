using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sushi.Room.Application.Constants;
using Sushi.Room.Application.Options;
using Sushi.Room.Application.Services;
using Sushi.Room.Domain.AggregatesModel.CategoryAggregate;
using Sushi.Room.Domain.AggregatesModel.ProductAggregate;
using Sushi.Room.Domain.AggregatesModel.UserAggregate;
using Sushi.Room.Domain.SeedWork;
using Sushi.Room.Infrastructure;
using Sushi.Room.Infrastructure.Repositories;
using Sushi.Room.Web.Infrastructure;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Sushi.Room.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining(typeof(Startup)).ConfigureClientsideValidation(enabled: false))
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUploadService, UploadService>();


            services.AddDbContext<SushiRoomDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString(nameof(SushiRoomDbContext)));
                options.UseLazyLoadingProxies();
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/admin/login";
                    options.LogoutPath = "/admin/logout";
                    options.AccessDeniedPath = "/admin/login";
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var cultures = new List<CultureInfo> {
                new CultureInfo(Cultures.ka),
                new CultureInfo(Cultures.en)
            };

            app.UseRequestLocalization(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Cultures.ka);
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
                options.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider() { Options = options });

            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=ka}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            });

            Migrate(app).GetAwaiter();
        }

        static async Task Migrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<SushiRoomDbContext>();
            await context.Database.MigrateAsync();

            await DbInitializer.Initialize(context);
        }
    }
}
