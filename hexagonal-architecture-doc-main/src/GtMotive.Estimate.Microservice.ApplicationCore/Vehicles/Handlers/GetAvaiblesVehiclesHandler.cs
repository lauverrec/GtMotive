using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Repository;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using GtMotive.Estimate.Microservice.Domain.Entities;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Handlers
{
    /// <summary>
    /// Get avaibles vehicles handler.
    /// </summary>
    public class GetAvaiblesVehiclesHandler : IRequestHandler<GetAvaiblesVehiclesQuery, IEnumerable<Vehicle>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAvaiblesVehiclesHandler"/> class.
        /// </summary>
        /// <param name="vehicleRepository">vehicleRepository.</param>
        public GetAvaiblesVehiclesHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="request">GetAvaiblesVehiclesQuery.</param>
        /// <param name="cancellationToken">cancellationToken.</param>
        /// <returns>void.</returns>
        public async Task<IEnumerable<Vehicle>> Handle(GetAvaiblesVehiclesQuery request, CancellationToken cancellationToken)
        {
            return await _vehicleRepository.GetAvaiblesVehiclesAsync();
        }
    }
}
