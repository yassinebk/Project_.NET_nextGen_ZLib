using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Project.Models;

namespace Project;
using Project.Data;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CoreModelsDataContext>(options =>
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        }); 
        services.AddControllersWithViews();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie();
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<CoreModelsDataContext>()
            .AddDefaultTokenProviders();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
  

        
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();

        app.UseAuthorization();
              app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
                    
    
    }
}