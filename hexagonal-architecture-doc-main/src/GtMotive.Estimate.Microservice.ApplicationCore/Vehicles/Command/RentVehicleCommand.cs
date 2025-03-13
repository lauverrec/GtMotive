using System;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Command
{
    /// <summary>
    /// Rent vehicle command.
    /// </summary>
    public class RentVehicleCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Vehicle id.
        /// </summary>
        public Guid VehicleId { get; set; }

        /// <summary>
        /// Gets or sets client id.
        /// </summary>
        public Guid ClientId { get; set; }
    }
}
