using System;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Command
{
    /// <summary>
    /// Return vehicle command.
    /// </summary>
    public class ReturnVehicleCommand : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets vehicle Id.
        /// </summary>
        public Guid VehicleId { get; set; }
    }
}
