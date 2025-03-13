using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Repository;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Infrastructure.Context;
using GtMotive.Estimate.Microservice.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GtMotive.Estimate.Microservice.Tests.Integration
{
    public class VehicleIntegrationTests
    {
        private readonly ServiceProvider _serviceProvider;

        public VehicleIntegrationTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("IntegrationTestDb"));

            services.AddScoped<IVehicleRepository, VehicleRepository>();

            _serviceProvider = services.BuildServiceProvider();

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task AddVehicleShouldPersistVehicleInDatabase()
        {
            using var scope = _serviceProvider.CreateScope();
            var vehicleRepository = scope.ServiceProvider.GetRequiredService<IVehicleRepository>();

            var vehicle = new Vehicle("TestBrand", "TestModel", 2020);

            await vehicleRepository.AddAsync(vehicle);

            var result = await vehicleRepository.GetByIdAsync(vehicle.Id);
            Assert.NotNull(result);
            Assert.Equal(vehicle.Brand, result.Brand);
            Assert.Equal(vehicle.Model, result.Model);
            Assert.Equal(vehicle.ManufacturingDate, result.ManufacturingDate);
        }
    }
}
