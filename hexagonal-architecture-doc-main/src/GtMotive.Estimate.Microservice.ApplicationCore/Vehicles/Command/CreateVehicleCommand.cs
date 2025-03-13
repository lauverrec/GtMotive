using GtMotive.Estimate.Microservice.Domain.Entities;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Command
{
    /// <summary>
    /// Create vehicle command.
    /// </summary>
    public class CreateVehicleCommand : IRequest<Vehicle>
    {
        /// <summary>
        /// Gets or sets vehicle brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets vehicle Model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets vehicle manufacturing date.
        /// </summary>
        public int ManufacturingDate { get; set; }
    }
}
