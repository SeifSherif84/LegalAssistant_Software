using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using Shared.Dtos.Authentications;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Domain.Contracts;
using Persistence;
using Services.Mapping.Authentications;
using Services.Abstractions;
using Services;
using Web.Middleware;
using Company.PL.Helper.MailKitFeature;
using Store.G02.Persistence;
using Services.Mapping.Lawyers;
using Services.Mapping.Cases;
using Services.Mapping.Documents;
using Microsoft.AspNetCore.Mvc;
using Services.Mapping.CourtSessions;
using Services.Mapping.ChatBot;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWTOptions"));

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


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
