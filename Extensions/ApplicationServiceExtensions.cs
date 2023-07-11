using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
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
            services.AddScoped<IUserRepository, UserRepository>();
            //Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Cloudinary
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            //Photos
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            //Likes
            services.AddScoped<ILikeRepository, LikeRepository>();
            //Messages
            services.AddScoped<IMessageRepository, MessageRepository>();
            //SignalR
            services.AddSignalR();

            return services;
        }
    }
}