using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Command;
using GtMotive.Estimate.Microservice.ApplicationCore.Repository;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Handlers
{
    /// <summary>
    /// Rent vehicle handler.
    /// </summary>
    public class RentVehicleHandler : IRequestHandler<RentVehicleCommand, bool>
    {
        private readonly IVehicleRepository _vehicleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleHandler"/> class.
        /// </summary>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        public RentVehicleHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Create vehicle handler.
        /// </summary>
        /// <param name="request">Create vehicle command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Vehicle id.</returns>
        public async Task<bool> Handle(RentVehicleCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new InvalidOperationException("The request is null");
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);

            if (vehicle == null || !vehicle.Avaible)
            {
                return false;
            }

            var vehicleRented = (await _vehicleRepository.GetAllAsync())
                .Where(v => !v.Avaible && v.ClientId == request.ClientId);

            if (vehicleRented.Any())
            {
                return false;
            }

            vehicle.Avaible = false;
            vehicle.ClientId = request.ClientId;
            await _vehicleRepository.UpdateAsync(vehicle);

            return true;
        }
    }
}
