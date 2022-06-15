using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using MusicStore.Components;
using MusicStore.Filters;
using MusicStore.Middlewares;
using MusicStore.Models;
using MusicStore.Services;

namespace MusicStore
{
    public class Startup
    {       

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            // Below code demonstrates usage of multiple configuration sources. For instance a setting say 'setting1'
            // is found in both the registered sources, then the later source will win. By this way a Local config
            // can be overridden by a different setting while deployed remotely.
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("config.json")
                //All environment variables in the process's context flow in as configuration values.
                .AddEnvironmentVariables();

            Configuration = builder.Build();           
        }

        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add EF services to the services container    
            string connectionString = Configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<MusicStoreContext>(optionsBuilder =>
            {
                optionsBuilder
                     .UseSqlServer(connectionString)
                     .EnableSensitiveDataLogging();
            }, ServiceLifetime.Singleton);

            // Add Identity services to the services container
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<MusicStoreContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://example.com");
                });
            });

            services.AddLogging();

            // Add MVC services to the services container

            services.AddMvc();
            // Add memory cache services
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            // Add session related services.
            services.AddSession();

            //ada custom filter
            services.AddScoped<AuditTrailsActionFilter>();
            services.AddScoped<OrderActionFilter>();
            services.AddScoped<MyCustomActionFilter>();
            services.AddScoped<MyResultFilter>();


            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(MyGlobalActionFilter));
            }).AddJsonOptions(opt => {
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            //DI
            //services.AddScoped<IMusicService, MusicService>(); // different at each request

            //services.AddSingleton<IMusicService, MusicService2>();
            services.AddSingleton<IMusicService, MusicService>();

            //services.
            //services.AddScoped<IMusicService, MusicService2>();
            //services.AddScoped<IMusicService, MusicService>();

            // ODATA
            services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Filter().Select());
            //services.AddControllers().AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()));

            // TryAdd - .Net 5
            // Life style: 
            // services.tr
            // A contain B: B life > A life
            // A singleton, B scope: B life < A life

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageStore",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("ManageStore", "Allowed");
                    });
            });
        }

        //This method is invoked when ASPNETCORE_ENVIRONMENT is 'Development' or is not defined
        //The allowed values are Development,Staging and Production
        public void ConfigureDevelopment(IApplicationBuilder app)
        {
            // StatusCode pages to gracefully handle status codes 400-599.
            app.UseStatusCodePagesWithRedirects("~/Home/StatusCodePage");

            // Display custom error page in production when error occurs
            // During development use the ErrorPage middleware to display error information in the browser
            app.UseDeveloperExceptionPage();

            app.UseDatabaseErrorPage();


            Configure(app);
        }

        //This method is invoked when ASPNETCORE_ENVIRONMENT is 'Staging'
        //The allowed values are Development,Staging and Production
        public void ConfigureStaging(IApplicationBuilder app)
        {
            // StatusCode pages to gracefully handle status codes 400-599.
            app.UseStatusCodePagesWithRedirects("~/Home/StatusCodePage");

            app.UseExceptionHandler("/Home/Error");

            Configure(app);
        }

        //This method is invoked when ASPNETCORE_ENVIRONMENT is 'Production'
        //The allowed values are Development,Staging and Production
        public void ConfigureProduction(IApplicationBuilder app)
        {
            // StatusCode pages to gracefully handle status codes 400-599.
            app.UseStatusCodePagesWithRedirects("~/Home/StatusCodePage");

            app.UseExceptionHandler("/Home/Error");

            Configure(app);
        }

        public void Configure(IApplicationBuilder app)
        {
            // force the en-US culture, so that the app behaves the same even on machines with different default culture
            var supportedCultures = new[] { new CultureInfo("en-US") };


            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Q: Usage in real project beside logging?
            // A: setting headers and a status code, etc.

            app.Map(new PathString("/Store/Details/9999"), config =>
            {
                config.Run(async context =>
                {
                    await context.Response.WriteAsync("Hello from app.Map() when Id=9999");
                });
            });

            // Status code page middleware

            app.UseMiddleware<MyMiddleware1>();
            app.UseMiddleware<MyMiddleware2>();
            //app.UseMiddleware<MessageHandler1>();

            //app.UseExceptionHandler("/Views/Shared/Error.cshtml");
            //app.UseMiddleware<MyMiddleware1>();
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ErrorMiddleware().Invoke
            });
            ///app.UseExceptionHandler(c => c.UseMiddleware<MyMiddleware2>());
            //app.useex(c => c.usemi);

            app.Use((context, next) =>
            {
                context.Response.Headers["Arch"] = RuntimeInformation.ProcessArchitecture.ToString();
                return next();
            });

            // Configure Session.
            app.UseSession();

            // Add static files to the request pipeline
            app.UseStaticFiles();

            // Add the endpoint routing matcher middleware to the request pipeline
            app.UseRouting();

            // Add cookie-based authentication to the request pipeline
            app.UseAuthentication();

            // Add the authorization middleware to the request pipeline
            app.UseAuthorization();

            // Add endpoints to the request pipeline
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "{controller}/{id?}");
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Stop all later middlewares");
            //});

            //// Never run
            //app.Use((context, next) =>
            //{
            //    return context.Response.WriteAsync("Cannot reach");
            //});

            //Populates the MusicStore sample data
            SampleData.InitializeMusicStoreDatabaseAsync(app.ApplicationServices).Wait();
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            //EntityTypeConfiguration<Genre> window = builder.EntityType<Genre>();
            //window.HasKey(c => c.GenreId);
            //window.ComplexProperty(c => c.BaseModelType);

            //ComplexTypeConfiguration<BaseModelType> baseModelType = builder.ComplexType<BaseModelType>();
            //baseModelType.Property(c => c.ModelType);
            //baseModelType.Abstract();

            //ComplexTypeConfiguration<ExternalType> externalType = builder.ComplexType<ExternalType>();
            //externalType.Property(c => c.Export);
            //externalType.DerivesFrom<BaseModelType>();

            //ComplexTypeConfiguration<InternalType> internalType = builder.ComplexType<InternalType>();
            //internalType.Property(c => c.InternalName);
            //internalType.DerivesFrom<BaseModelType>();

            builder.Namespace = "AlbumService";
            builder.EntityType<Album>()
                .Action("SetName")
                .Parameter<string>("NewName");

            builder.EntityType<Album>().Collection
                .Function("MostExpensive")
                .Returns<double>();

            // https://localhost:44374/odata
            builder.Singleton<Genre>("Rock");

            builder.EntitySet<Genre>("Genres");
            builder.EntitySet<Album>("Albums");
            builder.EntitySet<Artist>("Artists");


            var albumType = builder.EntityType<Album>();
            var functionConfiguration = albumType.Collection.Function("GetCount");
            functionConfiguration.Parameter<string>("NameContains");
            functionConfiguration.Returns<int>();

            return builder.GetEdmModel();
        }
    }
}
