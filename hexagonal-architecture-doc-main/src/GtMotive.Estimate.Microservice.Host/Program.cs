using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GtMotive.Estimate.Microservice.Api;
using GtMotive.Estimate.Microservice.Api.Filters;
using GtMotive.Estimate.Microservice.Api.Mappers;
using GtMotive.Estimate.Microservice.ApplicationCore.Repository;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Handlers;
using GtMotive.Estimate.Microservice.Host.Configuration;
using GtMotive.Estimate.Microservice.Host.DependencyInjection;
using GtMotive.Estimate.Microservice.Infrastructure;
using GtMotive.Estimate.Microservice.Infrastructure.Context;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using GtMotive.Estimate.Microservice.Infrastructure.Repository;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace GtMotive.Estimate.Microservice.Host// Aquí defines el namespace deseado.
{
#pragma warning disable CA1052 // Los tipos de contenedor estáticos deben ser Static o NotInheritable
    public class Program
#pragma warning restore CA1052 // Los tipos de contenedor estáticos deben ser Static o NotInheritable
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración.
            if (!builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile("serilogsettings.json", optional: false, reloadOnChange: true);

                var secretClient = new SecretClient(
                    new Uri($"https://{builder.Configuration.GetValue<string>("KeyVaultName")}.vault.azure.net/"),
                    new DefaultAzureCredential());

                builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
            }

            // Configuración de logging para el arranque del host.
            builder.Logging.ClearProviders();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .CreateBootstrapLogger();

            builder.Host.UseSerilog();

            // Agregar servicios al contenedor.
            if (!builder.Environment.IsDevelopment())
            {
                builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);
                builder.Services.AddApplicationInsightsKubernetesEnricher();
            }

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

            builder.Services.AddControllers(ApiConfiguration.ConfigureControllers)
                .WithApiControllers();

            builder.Services.AddBaseInfrastructure(builder.Environment.IsDevelopment());

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                           ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = appSettings.JwtAuthority;
                    options.ApiName = "estimate-api";
                    options.SupportedTokens = SupportedTokens.Jwt;
                });

            builder.Services.AddSwagger(appSettings, builder.Configuration);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
            builder.Services.AddMediatR(typeof(GetAvaiblesVehiclesHandler).Assembly);

            builder.Services.AddControllers();

            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddAutoMapper(typeof(VehicleProfile));

            builder.Services.AddScoped<BusinessExceptionFilter>();

            builder.Services.AddControllers(options =>
            {
                // Puedes agregar el filtro de forma global.
                options.Filters.Add<BusinessExceptionFilter>();
            });

            var app = builder.Build();

            // Configuración de logging.
            Log.Logger = builder.Environment.IsDevelopment() ?
                new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                        theme: AnsiConsoleTheme.Literate)
                    .CreateLogger() :
                new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "addoperation")
                    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                    .WriteTo.ApplicationInsights(
                        app.Services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();

            var pathBase = new PathBase(builder.Configuration.GetValue("PathBase", defaultValue: PathBase.DefaultPathBase));

            if (!pathBase.IsDefault)
            {
                app.UsePathBase(pathBase.CurrentWithoutTrailingSlash);
            }

            app.UseForwardedHeaders();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseSwaggerInApplication(pathBase, builder.Configuration);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
