using Company.PL.Helper.MailKitFeature;
using Domain.Contracts;
using Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Data.Contexts;
using Services;
using Services.Abstractions;
using Services.CourtSessions.Handlers;
using Services.Decisions;
using Services.Mapping.Authentications;
using Services.Mapping.Cases;
using Services.Mapping.ChatBot;
using Services.Mapping.CourtSessions;
using Services.Mapping.Decisions;
using Services.Mapping.Documents;
using Services.Mapping.Lawyers;
using Shared.Dtos.Authentications;
using Store.G02.Persistence;
using System.Text;
using Web.Middleware;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #region New swagger
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            #endregion


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWTOptions"));

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                typeof(DecisionService).Assembly,
                typeof(UpdateSessionDateHandler).Assembly
            ));


            var JWTOptions = builder.Configuration.GetSection("JWTOptions").Get<JWTOptions>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JWTOptions.Issuer,
                    ValidAudience = JWTOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptions.SecurityKey))
                };
            });

            builder.Services.AddIdentityCore<UserApp>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

            builder.Services.AddAutoMapper(Config =>
            {
                Config.AddProfile(new AuthenticationProfile());
                Config.AddProfile(new LawyerProfile(builder.Configuration));
                Config.AddProfile(new CaseProfile());
                Config.AddProfile(new DocumentProfile(builder.Configuration));
                Config.AddProfile(new CourtSessionProfile());
                Config.AddProfile(new ChatBotProfile());
                Config.AddProfile(new DecisionProfile());
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                                                   .SelectMany(V => V.Errors)
                                                   .Select(E => E.ErrorMessage)
                                                   .ToList();
                    return new BadRequestObjectResult(new
                    {
                        Message = "Invalid data. Please check the provided information.",
                        Errors = errors,
                    });
                };
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4300")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            builder.Services.Configure<MailKitSetting>(builder.Configuration.GetSection(nameof(MailKitSetting)));
            builder.Services.AddScoped<IMailService, MailService>();


            #region Old
            //builder.Services.AddHttpClient("RAGModelServiceClient", client =>
            //{
            //    var aiSettings = builder.Configuration.GetSection("AiSettings");
            //    client.BaseAddress = new Uri(aiSettings["BaseUrl"]);
            //    client.Timeout = TimeSpan.FromSeconds(double.Parse(aiSettings["TimeoutInSeconds"]));
            //}); 
            #endregion


            builder.Services.AddHttpClient("RAGModelServiceClient", client =>
            {
                var aiSettings = builder.Configuration.GetSection("AiSettings");
                client.BaseAddress = new Uri(aiSettings["BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is missing"));
                client.Timeout = TimeSpan.FromSeconds(double.Parse(aiSettings["TimeoutInSeconds"] ?? "120"));
            });

            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });


            var app = builder.Build();

            app.UseCors("CorsPolicy");


            using var ScopedServices = app.Services.CreateScope();
            var DbInitializer = ScopedServices.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.InitializerAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<CheckUserStatusMiddleware>();

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            app.MapControllers();



            app.Run();
        }
    }
}
