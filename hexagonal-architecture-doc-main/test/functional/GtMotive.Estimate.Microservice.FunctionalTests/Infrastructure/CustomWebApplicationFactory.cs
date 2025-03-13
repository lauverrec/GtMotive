using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.IO;

namespace GtMotive.Estimate.Microservice.FunctionalTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Ajusta el ContentRoot para que apunte al directorio del proyecto de la aplicación.
            var projectDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\src\GtMotive.Estimate.Microservice.Host"));
            builder.UseContentRoot(projectDir);

            base.ConfigureWebHost(builder);
        }
    }
}
