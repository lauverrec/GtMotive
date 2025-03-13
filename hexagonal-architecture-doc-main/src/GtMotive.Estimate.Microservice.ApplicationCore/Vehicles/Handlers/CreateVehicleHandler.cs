using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Exceptions;
using GtMotive.Estimate.Microservice.ApplicationCore.Repository;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Command;
using GtMotive.Estimate.Microservice.Domain.Entities;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Handlers
{
    /// <summary>
    /// Create vehicle handler.
    /// </summary>
    public class CreateVehicleHandler : IRequestHandler<CreateVehicleCommand, Vehicle>
    {
        private readonly IVehicleRepository _vehicleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVehicleHandler"/> class.
        /// </summary>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        public CreateVehicleHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Create vehicle handler.
        /// </summary>
        /// <param name="request">Create vehicle command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Vehicle id.</returns>
        public async Task<Vehicle> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new InvalidOperationException("Request is null");
            }

            if (DateTime.Now.Year - request.ManufacturingDate > 5)
            {
                throw new BusinessException("The manufacturing date vehicle can't be older that 5 years");
            }

            var vehicle = new Vehicle(request.Brand, request.Model, request.ManufacturingDate);

            await _vehicleRepository.AddAsync(vehicle);
            return vehicle;
        }
    }
}
