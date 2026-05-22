using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RazorLight;
using SMSSender.CronJop;
using SMSSender.Entities.Auth;
using SMSSender.Entities.Models;
using SMSSender.Hubs;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Auth;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.CronJop;
using SMSSender.Interfaces.Hub;
using SMSSender.Interfaces.Repositories;
using SMSSender.Reports.Interface;
using SMSSender.Reports.Service;
using SMSSender.Services;
using SMSSender.Services.Auth;
using SMSSender.Services.Common;
using SMSSender.Services.Repositories;
using System.Text;

namespace SMSSender.DI
{
    public static class DependencyInjection
    {
        private const string MyAllowSpecificOrigins = "_SMSSender";
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.Get<AppSettings>();
            services.Configure<AppSettings>(configuration);
            services.AddSingleton<IAppSettings>(sp => sp.GetRequiredService<IOptions<AppSettings>>().Value);
            services.AddDbContext<SMSDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(appSettings.ConnectionStrings.DBConnection);
            });

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins(appSettings.URLList).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            }).AddNewtonsoftJson();
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(appSettings.ConnectionStrings.DBConnection);
            });
            services.AddHangfireServer();
            services.AddAuthConfig(configuration);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISQLHelper, SQLHelper>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ICashBoxService, CashBoxService>();
            services.AddScoped<IWalletDetailService, WalletDetailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IHubNotificationService, HubNotificationService>();
            services.AddScoped<IWalletReminderService, WalletReminderService>();
            services.AddScoped<IReportService, ReportService>();
            

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
