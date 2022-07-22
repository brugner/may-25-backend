using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Options;
using May25.API.Core.Services;
using May25.API.Data.DbContexts;
using May25.API.Data.Repositories;
using May25.API.Data.Services;
using May25.API.Data.UnitsOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace May25.API.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
        {
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience
                };
            });
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IMakeService, MakeService>();
            services.AddScoped<IDbManagerService, DbManagerService>();
            services.AddScoped<ISeatRequestService, SeatRequestService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUrlService, UrlService>();
            services.AddScoped<IHtmlService, HtmlService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAlertService, AlertService>();

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IMakeRepository, MakeRepository>();
            services.AddScoped<ISeatRequestRepository, SeatRequestRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();

            // Http
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddAppDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "May25 API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
