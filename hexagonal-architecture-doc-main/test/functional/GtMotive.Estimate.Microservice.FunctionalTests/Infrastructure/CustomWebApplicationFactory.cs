using System.Linq;
using GtMotive.Estimate.Microservice.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Tests
{
    // Hereda de WebApplicationFactory utilizando tu clase de inicio (por ejemplo, Program)
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        // Sobrescribe el método ConfigureWebHost para modificar la configuración de la aplicación
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder?.ConfigureServices(services =>
                {
                    // Busca la descripción del servicio que registra el ApplicationDbContext
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        // Remueve la configuración original de la base de datos
                        services.Remove(descriptor);
                    }

                    // Registra el ApplicationDbContext usando una base de datos en memoria para pruebas
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    // Crea el ServiceProvider para construir el contexto y realizar configuraciones adicionales (por ejemplo, semilla de datos)
                    var sp = services.BuildServiceProvider();

                    // Opcional: crea un scope para obtener el contexto y asegurarte de que la base de datos esté creada y con datos iniciales
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    db.Database.EnsureCreated();
                });
        }
    }
}
