using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Command;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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

            // List of avaible vehicles
            var listResponse = await _client.GetAsync(new Uri("api/vehicles/available"));
            listResponse.EnsureSuccessStatusCode();

            // Read response
            var vehicles = await listResponse.Content.ReadFromJsonAsync<IEnumerable<Vehicle>>();

            // Verificar que la lista no sea nula y que contenga al vehículo creado.
            Assert.NotNull(vehicles);
            Assert.Contains(vehicles, v => v.Brand == "Audi" && v.Model == "A3");
        }
    }
}
