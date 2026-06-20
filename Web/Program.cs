using Company.PL.Helper.MailKitFeature;
using Domain.Contracts;
using Domain.Entities.Identity;
using Hangfire;
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
using Services.Abstractions.ChatBot;
using Services.Abstractions.MailKitFeature;
using Services.Abstractions.Persons;
using Services.ChatBot;
using Services.CourtSessions.Handlers;
using Services.Decisions;
using Services.MailKitFeature;
using Services.Mapping.Authentications;
using Services.Mapping.CaseParties;
using Services.Mapping.Cases;
using Services.Mapping.ChatBot;
using Services.Mapping.CourtSessions;
using Services.Mapping.Decisions;
using Services.Mapping.Documents;
using Services.Mapping.Lawyers;
using Services.Mapping.Persons;
using Services.Persons;
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
            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddScoped<IConanApiService, ConanApiService>();
            builder.Services.AddScoped<IChatBotService, ChatBotService>();
            builder.Services.AddScoped<ILegalAnalysisService, LegalAnalysisService>();
            builder.Services.AddScoped<IReminderJob, ReminderJob>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                typeof(DecisionService).Assembly,
                typeof(UpdateSessionDateHandler).Assembly
            ));

            // إضافة Hangfire
            builder.Services.AddHangfire(config =>
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHangfireServer();

            // Register الـ Job
            


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
                Config.AddProfile(new PersonProfile());
                Config.AddProfile(new CasePartyProfile());
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
                    builder.WithOrigins("http://localhost:4300", "https://mullets-shrill-violator.ngrok-free.dev")
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

            #region old http
            //builder.Services.AddHttpClient("RAGModelServiceClient", client =>
            //{
            //    var aiSettings = builder.Configuration.GetSection("AiSettings");
            //    client.BaseAddress = new Uri(aiSettings["BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is missing"));
            //    client.Timeout = TimeSpan.FromSeconds(double.Parse(aiSettings["TimeoutInSeconds"] ?? "120"));
            //});
            #endregion
            builder.Services.AddHttpClient("ConanServiceClient", client =>
            {
                client.BaseAddress = new Uri(
                    builder.Configuration["ConanApi:BaseUrl"]   // e.g. "http://localhost:8000/api/v1/"
                    ?? throw new InvalidOperationException("ConanApi:BaseUrl is not configured."));

                var timeoutSeconds = builder.Configuration.GetValue<double>("ConanApi:TimeoutSeconds", 120);
                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            });

            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });


            var app = builder.Build();

            // -------------------------------------------------------------
            // ترتيب الـ Middlewares الصحيح (الطلب يمر من الأعلى للأسفل)
            // -------------------------------------------------------------

            // أولاً: الـ CORS يجب أن يتم استدعاؤه في البداية لمعالجة طلبات الـ OPTIONS الفورية من المتصفحات
            app.UseCors("CorsPolicy");

            // ثانياً: معالجة الأخطاء العالمية لتأمين أي مشكلة تحدث لاحقاً
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            // ثالثاً: التحقق من الهوية والصلاحيات
            app.UseAuthentication();
            app.UseAuthorization();

            // رابعاً: الـ Custom Middlewares الخاصة بمشروعك تأتي بعد التأكد من الهوية والـ CORS
            app.UseMiddleware<CheckUserStatusMiddleware>();

            // خامساً: إعدادات الخلفية والـ Dashboard للـ Hangfire
            app.UseHangfireDashboard("/hangfire");

            RecurringJob.AddOrUpdate<IReminderJob>(
                "session-reminders",
                job => job.SendPendingRemindersAsync(),
                Cron.Minutely
            );

            using var ScopedServices = app.Services.CreateScope();
            var DbInitializer = ScopedServices.ServiceProvider.GetRequiredService<IDbInitializer>();
            await DbInitializer.InitializerAsync();

            // سادساً: توجيه الطلبات إلى الـ Controllers
            app.MapControllers();

            app.Run();
        }
    }
}
