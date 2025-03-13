using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Repository;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Command;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Handlers
{
    /// <summary>
    /// Return vehicle handler.
    /// </summary>
    public class ReturnVehicleHandler : IRequestHandler<ReturnVehicleCommand, bool>
    {
        private readonly IVehicleRepository _vehicleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnVehicleHandler"/> class.
        /// </summary>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        public ReturnVehicleHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Return vehicle handler.
        /// </summary>
        /// <param name="request">Return vehicle command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the operation is sucessfull.</returns>
        /// <exception cref="InvalidOperationException">If request is null.</exception>
        public async Task<bool> Handle(ReturnVehicleCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new InvalidOperationException("The request is null");
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);

            if (vehicle == null || vehicle.Avaible)
            {
                return false;
            }

            vehicle.Avaible = true;
            vehicle.ClientId = null;
            await _vehicleRepository.UpdateAsync(vehicle);

            return true;
        }
    }
}
