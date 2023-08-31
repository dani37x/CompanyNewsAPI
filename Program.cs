using CompanyNewsAPI.Data;
using CompanyNewsAPI.Helpers;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Repositories;
using CompanyNewsAPI.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace CompanyNewsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register Transient Services
            builder.Services.AddTransient<IUserRepo, UserRepo>();
            builder.Services.AddTransient<IPostRepo, PostRepo>();
            builder.Services.AddTransient<IAuthRepo, AuthRepo>();

            // Register Singleton
            builder.Services.AddSingleton(provider =>
            {
                var email = builder.Configuration.GetValue<string>("EmailService:email");
                var password = builder.Configuration.GetValue<string>("EmailService:password");
                return new EmailService(email, password);
            });

            // Hangfire config
            builder.Services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddHangfireServer();

            // JSON Web Token Authentication settings
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key")))
                };
                options.SaveToken = true;
            });

            builder.Services.AddControllers();

            // Cross-Origin Resource Sharing settings
            builder.Services.AddCors(options => options.AddPolicy(name: "CompanyNewsUI",
                policy => policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

            // Database implementation
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddHealthChecks();

            // Swagger Authorization implementation & settings
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard();

            app.UseCors("CompanyNewsUI");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapHealthChecks("/health");
            //});

            // Hangfire workers
            RecurringJob.AddOrUpdate<ExpiredDataCleaner>("clean-expired-registration-keys", job => job.CleanExpiredData(@"registrationKeys.json"), Cron.Hourly());
            RecurringJob.AddOrUpdate<ExpiredDataCleaner>("clean-expired-newPassword-keys", job => job.CleanExpiredData(@"newPasswordKeys.json"), Cron.Hourly());
            RecurringJob.AddOrUpdate<ExpiredDataCleaner>("clean-expired-errors", job => job.CleanExpiredData(@"errors.json"), Cron.Hourly());

            // Exception Middleware implementation
            app.UseMiddleware<ExceptionHandlerService>();

            app.MapControllers();

            app.Run();
        }
    }
}