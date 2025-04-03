using DrugKitAPI.Core.Helpers;
using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Services;
using DrugKitAPI.EF;
using DrugKitAPI.EF.Data;
using DrugKitAPI.EF.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DrugKitAPI.Core.Models;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

namespace DrugKitAPI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Your API Title",
                    Version = "v1",
                    Description = "API Description"
                });

                // إذا كنت تحتاج إلى المصادقة
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });
            builder.Services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Your API Name";
                configure.Version = "v1";
                configure.Description = "Your API Description";

                // إذا كنت تحتاج إلى إعداد المصادقة
                configure.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token"
                });
            });





            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddTransient<HttpClient, HttpClient>();
            builder.Services.AddTransient<IUnitOFWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddControllers();
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.Configure<AppSetteings>(builder.Configuration.GetSection("AppSetteings"));
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IMailingService, MailingService>();
            builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAISettings"));
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IOpenAIService, OpenAIService>();
            builder.Services.AddScoped<IDrugService, DrugService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IDrugCsvService, DrugCsvService>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthenticatedUser", policy =>
                    policy.RequireAuthenticatedUser());
            });
            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"]
                };

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Check if the token is available in cookies or query string (for SignalR WebSocket)
                        var token = context.Request.Cookies["token"] ?? context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token; // Assign the token
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });


            // AutoMapper configuration
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(DrugKitAPI.Core.Helpers.MappingProfile));

            // Register IActionContextAccessor and IUrlHelper
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddTransient<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddOpenApiDocument();
            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            app.UseOpenApi(options =>
            {
                options.Path = "/openapi/{documentName}.json";
            });
            app.MapScalarApiReference();
            //}
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();

            //}
            
            app.UseHttpsRedirection();

            app.UseCors("AllowAll");


            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
