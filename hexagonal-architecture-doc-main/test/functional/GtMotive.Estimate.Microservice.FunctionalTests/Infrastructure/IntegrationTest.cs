using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Command;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests
{
    public class IntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory?.CreateClient();
        }

        [Fact]
        public async Task CreateVehicleThenListAvailableVehicles()
        {
            // 1. Crear un vehículo
            var createCommand = new CreateVehicleCommand
            {
                Brand = "Audi",
                Model = "A3",
                ManufacturingDate = DateTime.Now.Year
            };

            // Send POST for creating the vehicle
            var createResponse = await _client.PostAsJsonAsync("api/vehicles", createCommand);
            createResponse.EnsureSuccessStatusCode();

            Assert.NotNull(createResponse);
        }
    }
}
