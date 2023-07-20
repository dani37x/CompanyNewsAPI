using CompanyNewsAPI.Data;
using CompanyNewsAPI.Interfaces;
using CompanyNewsAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace CompanyNewsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IUserRepo, UserRepo>();
            builder.Services.AddTransient<IPostRepo, PostRepo>();
            //builder.Services.AddTransient<IAuthRepo, AuthRepo>();

            builder.Services.AddControllers();
            builder.Services.AddCors(options => options.AddPolicy(name: "CompanyNewsUI",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                }));

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddHealthChecks();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CompanyNewsUI");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHealthChecks("/health");
            //});

            app.MapControllers();

            app.Run();


        }

    }
    
}