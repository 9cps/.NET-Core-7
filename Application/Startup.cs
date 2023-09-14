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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy("AllowOrigin", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));
        // Add other service configurations
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Professional API", Version = "v1" });

            // Configure Bearer token authentication
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter 'Bearer {token}'",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        // Use reflection to automatically register all interfaces and their implementations
        services.AddTransient<IMasterService, MasterServiceController>();
        services.AddSingleton(Configuration);
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

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),
                ValidAudience = Configuration.GetValue<string>("JwtSettings:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSettings:Key")!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
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
        app.UseCors("AllowOrigin");

        // Add culture configuration middleware
        var supportedCultures = new[] { new CultureInfo("en-US") }; // Replace with your desired culture(s)
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-US"), // Replace with your desired culture
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        app.UseRouting();

        // Authentication and Authorization middleware should be outside of the if block
        app.UseAuthentication(); // Should be before other middlewares that might require authentication.
        app.UseAuthorization();

        if (env.IsDevelopment())
        {
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<Middleware>();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
