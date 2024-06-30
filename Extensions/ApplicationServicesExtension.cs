using Microsoft.EntityFrameworkCore;
using OrderNumberSequence.DATA;
using OrderNumberSequence.Helpers;
using OrderNumberSequence.Interface;
using OrderNumberSequence.Respository;
using OrderNumberSequence.Services;
using OrderNumberSequence.Repository;

namespace OrderNumberSequence.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(
            options => options.UseNpgsql(config.GetConnectionString("local")));
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<IUserService, UserService>();
        // here to add
services.AddScoped<IOrderServices, OrderServices>();
services.AddScoped<IProductServices, ProductServices>();
services.AddScoped<IMessageServices, MessageServices>();
        services.AddScoped<IFileService, FileService>();


        // seed data from permission seeder service


        return services;
    }
}
