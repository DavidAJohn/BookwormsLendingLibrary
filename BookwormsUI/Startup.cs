using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookwormsUI.Contracts;
using BookwormsUI.Repository;
using BookwormsUI.Services;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BookwormsUI.Providers;
using Blazored.Toast;
using Blazored.Modal;

namespace BookwormsUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<SettingsService>();
            services.AddSingleton<AuthorService>();
            services.AddSingleton<BookService>();
            services.AddSingleton<CategoryService>();
            services.AddScoped<RequestService>();

            services.AddAuthorizationCore();
            services.AddHttpContextAccessor();

            services.AddHttpClient<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ApiAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            services
                .AddBlazorise( options =>
                {
                    options.ChangeTextOnKeyPress = true;
                } )
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            services.AddBlazoredLocalStorage();
            services.AddBlazoredToast();
            services.AddBlazoredModal();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
               
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
