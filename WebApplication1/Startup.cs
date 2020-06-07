using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Ardalis.ListStartupServices;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.WebApplication1.Services;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Microsoft.WebApplication1.Data;
using Microsoft.WebApplication1.Identity;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Microsoft.WebApplication1;
using Microsoft.WebApplication1.Logging;
using Microsoft.WebApplication1.Pages.Basket;
using System.Reflection;

namespace WebApplication1
{
    public class Startup
    { 

        protected IServiceCollection _services;
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigurationDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );


            //this is for using real database 
            services.AddDbContext<AppIdentityDbContext>(options =>
         // this context creates Users, Roles, and UserTokens tables in db
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
        }

        public void ConfigureInMemoryDatabase(IServiceCollection services)
        {
            // using in-memory database
            services.AddDbContext<CatalogContext>(c => c.UseInMemoryDatabase("Catalog"));

            //add identity DbContext 
            services.AddDbContext<AppIdentityDbContext>(options => options.UseInMemoryDatabase("Identiy"));

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            //Steps to creating db tables 
            /*
             1. run ```dotnet ef database update -c <contextName> -p <projectFilePath> -s <projectFilePath>
             2. run ```Add-Migration Initial<Model/IdentityModel> -Context <contextName>
             3. run ```dotnet ef database update --context <contextName>
             */
            //using real database(LocalDB)

            services.AddDbContext<CatalogContext>(c =>
            // 
                c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddDbContext<AppIdentityDbContext>(options =>
            // this context creates Users, Roles, and UserTokens tables in db
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigurationDevelopmentServices(services);
            ConfigureCookieSettings(services);

            services.AddIdentity<ApplicationUser, IdentityRole>() // setting ApplicationUser and IdentityRole as default User and Role types 
                .AddDefaultUI()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();
            services.AddMediatR(typeof(BasketViewModelService).Assembly);
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketViewModelService, BasketViewModelService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<CatalogViewModelService>();
            services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
            services.Configure<CatalogSettings>(Configuration);
            services.AddSingleton<IUriComposer>(new UriComposer(Configuration.Get<CatalogSettings>()));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMemoryCache();

            services.AddRouting(options =>
            {
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });

            services.AddMvc(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new SlugifyParameterTransformer())); 
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Basket/Checkout");
            });

            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Shehan's API", Version = "v1" }));

            services.AddHealthChecks();
            services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(services);
                config.Path = "/allServices"; 
            });

            _services = services;
        }

        private static void ConfigureCookieSettings(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true // IsEssential is required for auth to work without explicit user consent, adjust to suit privacy policy when needed
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = async(context, report) =>
                    {
                        var result = JsonConvert.SerializeObject(
                            new
                            {
                                status = report.Status.ToString(),
                                errors = report.Entries.Select(e => new
                                {
                                    key = e.Key,
                                    value = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseShowAllServicesMiddleware();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerUI();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller:slugify=Home}/{action:slugify=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("home_page_health_check");
                endpoints.MapHealthChecks("api_health_check");
            });

        }
    }
}
