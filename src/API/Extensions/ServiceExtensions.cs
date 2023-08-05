using Domain.Entities;
using Infrastructure.Data.AppDbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Middlewares;
using FluentValidation.AspNetCore;
using Application.Validations;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using API.Configurations;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Infrastructure.Contracts;
using Application.Services;
using Application.Contracts;
using Application.Helpers;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Application.Validations;
using Application.Services;
using Application.Contracts;
using Domain.Entities.Identities;
using Infrastructure.Contracts;
using Infrastructure;
using Infrastructure.Contracts;
using Infrastructure.Repositories;
using Infrastructure.Data.Utils.Storage;
using Hangfire;
using Infrastructure.Data.Utils.Email;
using System.Text.Json.Serialization;
using Application.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace API.Extensions
{
    public static class ServiceExtensions
    {
        private static readonly ILoggerFactory ContextLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public static void ConfigureKestrelForGrpc(this WebApplicationBuilder builder)
        {
            var http = Convert.ToInt32(builder.Configuration.GetRequiredSection("Kestrel:Ports:Http:Url").Value);
            var https = Convert.ToInt32(builder.Configuration.GetRequiredSection("Kestrel:Ports:Https:Url").Value);
            var grcp = Convert.ToInt32(builder.Configuration.GetRequiredSection("Kestrel:Ports:Grcp:Url").Value);


            if (http <= 0 || grcp <= 0)
                throw new Exception("Http and/or Grcp ports not set");

            builder.WebHost.ConfigureKestrel(options =>
                {
                    if (http > 0)
                        options.ListenAnyIP(http, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1;
                        });

                    if (https > 0)
                        options.ListenAnyIP(https, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1;
                        listenOptions.UseHttps();
                    });

                    // Listen on any IP address assigned to the server on port 50001 with HttpProtocols.Http2
                    if (grcp > 0)
                        options.ListenAnyIP(50001, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });
                });
        }

        public static void ConfigureControllers(this IServiceCollection serviceCollection) =>
            serviceCollection.AddControllers()
                            .AddXmlDataContractSerializerFormatters()
                            .AddJsonOptions(x =>
                                {
                                    // serialize enums as strings in api responses (e.g. Role)
                                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                                }
                            );
        public static void ConfigureCors(this IServiceCollection serviceCollection) =>
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination"));
            });

        public static void ConfigureIisIntegration(this IServiceCollection serviceCollection) =>
            serviceCollection.Configure<IISOptions>(options => { });

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(connString));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        }
        public static void ConfigureAzureStorageServices(this IServiceCollection services)
        {
            services.AddTransient<IAzureStorage, AzureStorage>();
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var jwtUserSecret = jwtSettings.GetSection("Secret").Value;

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("ValidIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("ValidAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtUserSecret))
                };
            });
        }

        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc().ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = context => new ValidationFailedResult(context.ModelState);
            });
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<TenantValidationFilter>();
        }

        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IWebHelper, WebHelper>();
        }

        public static void ConfigureApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddMvcCore().AddApiExplorer();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.OperationFilter<RemoveVersionFromParameter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });

            });
        }

        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x =>
                x.UseSqlServerStorage(configuration.GetConnectionString("HangfireDbConnection")));
            services.AddHangfireServer();
        }
    }
}
