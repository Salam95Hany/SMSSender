using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SMSSender.Entities.Models;
using SMSSender.Services.Common;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Repositories;
using SMSSender.Interfaces.Auth;
using SMSSender.Services.Auth;
using SMSSender.Entities.Auth;
using RazorLight;
using SMSSender.Reports.Interface;
using SMSSender.Reports.Service;
using SMSSender.Interfaces;
using SMSSender.Services;

namespace SMSSender.DI
{
    public static class DependencyInjection
    {
        private const string MyAllowSpecificOrigins = "_SMSSender";
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<AppSettings>(configuration);
            services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);
            services.AddDbContext<SMSDbContext>((serviceProvider, options) =>
            {
                var appSettings = serviceProvider.GetRequiredService<IAppSettings>();
                options.UseSqlServer(appSettings.ConnectionStrings.DBConnection);
            });

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    var appSettings = services.BuildServiceProvider().GetRequiredService<IAppSettings>();
                    builder.WithOrigins(appSettings.URLList).AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            }).AddNewtonsoftJson();
            services.AddAuthConfig(configuration);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMessageService, MessageService>();


            #region ReportsDI

            services.AddSingleton<IRazorLightEngine>(serviceProvider =>
            {
                var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                var templatePath = Path.Combine(env.WebRootPath, "TemplatesHTML");
                return new RazorLightEngineBuilder()
                    .UseFileSystemProject(templatePath)
                    .UseMemoryCachingProvider()
                    .Build();
            });
            services.Scan(scan => scan.FromApplicationDependencies().AddClasses(c => c.AssignableTo<IReportGenerator>()).AsImplementedInterfaces().WithTransientLifetime());
            services.AddScoped<IReportGeneratorFactory, ReportGeneratorFactory>();
            services.AddScoped<IExportManagerService, ExportManagerService>();
            services.AddSingleton<IPDFHelper, PDFHelper>();

            #endregion

            return services;
        }

        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddIdentity<AdminUser, IdentityRole>().AddEntityFrameworkStores<SMSDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var appSettings = serviceProvider.GetRequiredService<IAppSettings>();
                var jwt = appSettings.Jwt;

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        public static string GetCorsPolicyName() => MyAllowSpecificOrigins;
    }
}
