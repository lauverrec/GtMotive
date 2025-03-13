namespace GtMotive.Estimate.Microservice.ApplicationCore.Interfaces.Dtos
{
    /// <summary>
    /// Vehicle DTO.
    /// </summary>
    public class VehicleDto
    {
        /// <summary>
        /// Gets or sets vehicle id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets brand vehicle.
        /// </summary
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets model Vehicle.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets manufacturing date vehicle.
        /// </summary>
        public string ManufacturingDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets if vehicle is avaible.
        /// </summary>
        public bool Avaible { get; set; }

        /// <summary>
        /// Gets or sets client id.
        /// </summary>
        public string ClientId { get; set; }
    }
}
