using Microsoft.Extensions.DependencyInjection;

namespace BookwormsAPI.Extensions
{
    public static class CorsServiceExtension
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Pagination"));
            });

            return services;
        }
    }
}