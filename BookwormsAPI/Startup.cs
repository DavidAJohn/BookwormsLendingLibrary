using AutoMapper;
using BookwormsAPI.Data;
using BookwormsAPI.Data.Identity;
using BookwormsAPI.Extensions;
using BookwormsAPI.Helpers;
using BookwormsAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace BookwormsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<AppIdentityDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            
            services.AddApplicationServices(); // extension method : ApplicationServicesExtensions

            services.AddIdentityServices(Configuration); // extension method : IdentityServiceExtensions

            services.AddSwaggerDocumentation(); // extension method : SwaggerServiceExtensions
            
            services.AddCorsConfiguration(); // extension method : CorsServiceExtensions

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddHttpContextAccessor();

            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>(); // custom exception middleware
            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // when requested path is not found
            
            if (env.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseResponseCaching();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
