namespace MedUnify.HealthPulseAPI
{
    using FluentValidation.AspNetCore;

    using MedUnify.HealthPulseAPI.DbContext;
    using MedUnify.HealthPulseAPI.Infrastructure.AutoMapper;
    using MedUnify.HealthPulseAPI.Infrastructure.Filters;
    using MedUnify.HealthPulseAPI.Infrastructure.Handlers;
    using MedUnify.HealthPulseAPI.Infrastructure.Middlewares;
    using MedUnify.HealthPulseAPI.Repositories;
    using MedUnify.HealthPulseAPI.Repositories.Concrete;
    using MedUnify.HealthPulseAPI.Repositories.Interface;
    using MedUnify.HealthPulseAPI.Services.Concrete;
    using MedUnify.HealthPulseAPI.Services.Interface;
    using MedUnify.ResourceModel;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using NLog.Web;
    using System.Text;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure NLog
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            var builder = WebApplication.CreateBuilder(args);
            // Access configuration
            var configuration = builder.Configuration;

            string absoluteProjectFolderPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\Database\"));

            string sqlConnectionString = builder.Configuration.GetConnectionString("MedUnifyDb");

            sqlConnectionString = sqlConnectionString.Replace("__ReplaceWithProjectFolder__", absoluteProjectFolderPath);

            // Register repositories
            builder.Services.AddDbContext<MedUnifyDbContext>(options =>
                options.UseSqlServer(sqlConnectionString));

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IVisitRepository, VisitRepository>();
            builder.Services.AddScoped<IProgressNoteRepository, ProgressNoteRepository>();

            // Register services
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IVisitService, VisitService>();
            builder.Services.AddScoped<IProgressNoteService, ProgressNoteService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

            builder.Services.AddScoped<IOrganizationHandler, OrganizationHandler>();
            builder.Services.AddScoped<OrganizationIdValidationFilter>();

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(LogPageRequestAttribute));
                options.Filters.Add(typeof(LogExceptionAttribute));
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ResourceModels>())
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            builder.Host.UseNLog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder => builder
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .WithExposedHeaders("x-request-id"));

            // Add RequestIdMiddleware to the pipeline
            app.UseMiddleware<RequestIdMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}