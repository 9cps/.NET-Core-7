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
        if (env.IsDevelopment())
        {
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<Middleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Add culture configuration middleware
        var supportedCultures = new[] { new CultureInfo("en-US") }; // Replace with your desired culture(s)
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"), // Replace with your desired culture
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseAuthorization();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
