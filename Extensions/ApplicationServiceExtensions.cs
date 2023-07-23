using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //Connect SQLite
            services.AddDbContext<DataContext>(opt => opt.UseSqlite(config.GetConnectionString("DefaultConnection")));
            services.AddCors();
            //Token
            services.AddScoped<ITokenService, TokenService>();
            //Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Cloudinary
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            //Photos
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            //SignalR
            services.AddSignalR();
            //action Status
            services.AddSingleton<PresenceTracker>();
            //Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}