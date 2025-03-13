using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Repository;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Handlers;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using GtMotive.Estimate.Microservice.Domain.Entities;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Handlers
{
    public class GetAvaiblesVehiclesHandlerTests
    {
        [Fact]
        public async Task HandleReturnsAvailableVehicles()
        {
            // Arrange
            var mockRepo = new Mock<IVehicleRepository>();
            var availableVehicles = new List<Vehicle>
            {
                new("Toyota", "Corolla", 2020)
            };

            mockRepo.Setup(repo => repo.GetAvaiblesVehiclesAsync()).ReturnsAsync(availableVehicles);
            var handler = new GetAvaiblesVehiclesHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetAvaiblesVehiclesQuery(), CancellationToken.None);

            // Assert
            Assert.Single(result);
        }
    }
}
