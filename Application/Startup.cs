using Application.Middleware;
using MasterService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using MasterService.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add other service configurations
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        // Use reflection to automatically register all interfaces and their implementations
        services.AddTransient<IMasterService, MasterServiceController>();
        services.AddSingleton(Configuration);
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Local authentication scheme
            options.DefaultSignInScheme = GoogleDefaults.AuthenticationScheme; // External authentication scheme (Google)
        })
        .AddCookie()
        .AddGoogle(options =>
        {
            options.ClientId = "67599451553-jg86t02t5tmsm23feio370q9cttg6iv0.apps.googleusercontent.com";
            options.ClientSecret = "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";
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.CallbackPath = "/Authen/GoogleCallback";
        });

        // Set the default culture
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("th-TH");
            options.SupportedCultures = new List<CultureInfo> { new CultureInfo("th-TH") };
            options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("th-TH") };
        });
        // Add other service configurations
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseCors();

        // Add culture configuration middleware
        var supportedCultures = new[] { new CultureInfo("en-US") }; // Replace with your desired culture(s)
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"), // Replace with your desired culture
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        // Authentication and Authorization middleware should be outside of the if block
        //app.UseAuthentication(); // Should be before other middlewares that might require authentication.
        app.UseAuthorization();

        if (env.IsDevelopment())
        {
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<Middleware>();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
