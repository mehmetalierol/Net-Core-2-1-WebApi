using AutoMapper;
using Company.Application.Common.Paging;
using Company.Application.Common.Paging.Interface;
using Company.Application.Common.UnitofWork;
using Company.Application.Data.Context;
using Company.Application.Data.Entities;
using Company.Application.WebApi.Controllers;
using Company.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Company.Application.WebApi
{
    public class Startup
    {
        #region Variables

        private IConfiguration _config { get; }

        #endregion Variables

        #region Constructor

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        #endregion Constructor

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region JwtTokenSection

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RoleClaimType = "Roles",
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Tokens:Issuer"],
                    ValidateIssuer = true,
                    ValidAudience = _config["Tokens:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                };
            });

            services.ConfigureApplicationCookie(options => options.LoginPath = "/api/Token");

            #endregion

            #region LoggingSection

            //appsettings.json dosyasında bulunan Logging ayarları olarak hangi seviyede loglama yapılacağını bildiriyor ve sonradan eklediğimiz
            //FilePrefix, LogDirectory, FileSizeLimit alanlarını da manuel olarak ayar dosyasından çekerek gerekli atamaları yapıyoruz
            //Böylece bu ayarları değiştirmek için projeyi yeniden deploy etmemiz gerekmeyecek.
            services.AddLogging(builder => builder.AddFile(options =>
            {
                options.FileName = _config["Logging:Options:FilePrefix"]; // Log dosyasının isminin nasıl başlayacağını belirtiyoruz
                options.LogDirectory = _config["Logging:Options:LogDirectory"]; // Log dosyaları hangi klasöre yazılacak
                options.FileSizeLimit = int.Parse(_config["Logging:Options:FileSizeLimit"]); // Maksimum log dosya boyutu ne kadar olacak, byte üzerinden hesaplanır. (appsettings.json dosyasında bu değer 20971520 olarak belirledik. Bu değer 20 megabyte ın byte halidir.)
            }));

            #endregion LoggingSection

            #region ApplicationDbContextSection

            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<DbContext, ApplicationDbContext>();

            #endregion ApplicationDbContextSection

            #region DependencyInjectionSection

            //Dependency injection ile çözümleme yapabilmek için burada hangi interface üzerinden projede instance alınmak istenirse hangi sınıfın dönüleceğini belirliyoruz.
            //eğer buradaki bağımlılıklardan birini değiştireceksek tek yapmamız gereken o interface'e karşılık gelen sınıfı değiştirmek olacak.
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddTransient(typeof(IPagingLinks<>), typeof(PagingLinks<>));
            services.AddScoped<ILanguageController, LanguageController>();
            services.AddScoped<IUserController, UserController>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                                           .ActionContext;
                return new UrlHelper(actionContext);
            });

            #endregion DependencyInjectionSection

            #region IdentitySection

            //Identity yapısını ekliyoruz ve kendi oluşturduğumuz sınıfları veriyoruz.
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddRoleManager<RoleManager<ApplicationRole>>()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            //Identity ile ilgili kurallar
            services.Configure<IdentityOptions>(options =>
            {
                // Parola ayarları
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Kullanıcının kilitlenmesi ile ilgili kurallar
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                // Yeni kullanıcı kuralları
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            });

            #endregion IdentitySection

            #region AutoMapperSection

            //Auto mapper'ı ekliyoruz
            services.AddAutoMapper();

            #endregion AutoMapperSection

            #region CorsSection

            //Cors ayarları
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    //.WithOrigins("http://localhost:4200")
                    .AllowAnyOrigin()
                    //.WithMethods("GET", "POST")
                    .AllowAnyMethod()
                    //.WithHeaders("accept", "content-type", "origin", "No-Auth")
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            #endregion CorsSection

            #region MvcSection

            services.AddMvc();

            #endregion MvcSection
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region EnvironmentSection

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #endregion EnvironmentSection

            #region IdentitySection

            app.UseAuthentication();

            #endregion IdentitySection

            #region CorsSection

            app.UseCors("CorsPolicy");

            #endregion CorsSection

            #region MvcSection

            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}");
            });

            #endregion MvcSection
        }
    }
}