using CompanyNewsAPI.Data;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Repositories;
using CompanyNewsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using CompanyNewsAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CompanyNewsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IUserRepo, UserRepo>();
            builder.Services.AddTransient<IPostRepo, PostRepo>();
            builder.Services.AddTransient<IAuthRepo, AuthRepo>();

            builder.Services.AddSingleton(provider =>
            {
                var email = builder.Configuration.GetValue<string>("EmailService:email");
                var password = builder.Configuration.GetValue<string>("EmailService:password");
                return new EmailService(email, password);
            });


            builder.Services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddHangfireServer();

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
            builder.Services.AddCors(options => options.AddPolicy(name: "CompanyNewsUI",
                policy => policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddHealthChecks();


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

            // Configure the HTTP request pipeline.
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

            RecurringJob.AddOrUpdate<ExpiredDataCleaner>("clean-expired-keys", job => job.CleanExpiredData(@"keys.json"), Cron.Hourly());
            RecurringJob.AddOrUpdate<ExpiredDataCleaner>("clean-expired-errors", job => job.CleanExpiredData(@"errors.json"), Cron.Minutely());

            app.UseMiddleware<ExceptionHandlerService>();

            app.MapControllers();

            app.Run();


        }

    }

}