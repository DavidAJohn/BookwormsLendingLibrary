using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BookwormsAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Bookworms Lending Library API", 
                    Version = "v1" 
                });
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
    }
}