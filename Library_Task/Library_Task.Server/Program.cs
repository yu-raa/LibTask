using AutoMapper;
using Library_Task.Server.DTO;
using Library_Task.Server.Frameworks;
using Library_Task.Server.UnitOfWork;
using Library_Task.Server.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modsen_Library_Test_Task.Entities;
using System.Text;

namespace Library_Task.Server
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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LibraryContext>(options =>
   options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["ConnStr"]));


            builder.Services.AddIdentity<DatabaseUser, IdentityRole>(options => {}).AddEntityFrameworkStores<LibraryContext>().AddDefaultTokenProviders();

            builder.Services.AddAutoMapper(typeof(Profile), typeof(MappingProfile));

            builder.Services.AddTransient(typeof(IUnitOfWork<BusinessEntity>), typeof(UnitOfWork<BusinessEntity>));

            builder.Services.AddTransient(typeof(IUnitOfWork<Book>), typeof(UnitOfWork<Book>));

            builder.Services.AddTransient(typeof(IUnitOfWork<Author>), typeof(UnitOfWork<Author>));

            builder.Services.AddTransient(typeof(IUnitOfWork<Reader>), typeof(UnitOfWork<Reader>));

            builder.Services.AddTransient(typeof(IReadServiceAsync<Book>), typeof(LibraryService<Book>));

            builder.Services.AddTransient(typeof(IReadServiceAsync<BusinessEntity>), typeof(LibraryService<BusinessEntity>));

            builder.Services.AddTransient(typeof(IServiceAsync<BusinessEntity>), typeof(LibraryService<BusinessEntity>));

            builder.Services.AddTransient(typeof(IReadServiceAsync<Author>), typeof(LibraryService<Author>));

            builder.Services.AddTransient(typeof(IServiceAsync<Book>), typeof(LibraryService<Book>));

            builder.Services.AddTransient(typeof(IServiceAsync<Author>), typeof(LibraryService<Author>));

            builder.Services.AddSingleton<IAuthorizationHandler, UserHandler>();

            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:secret"]);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddAuthorizationBuilder().AddPolicy("AdminPolicy", policy =>
        policy.Requirements.Add(new UserRequirement("admin")));

            var app = builder.Build();

            app.UseCors(opt => opt.AllowAnyHeader().AllowAnyOrigin());

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
