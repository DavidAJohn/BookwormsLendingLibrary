using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookwormsAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}