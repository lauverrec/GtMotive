using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Vehicle class.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="brand">vehicle brand.</param>
        /// <param name="model">Vehicle model.</param>
        /// <param name="manufacturingDate">Manufacturing date's vehicle.</param>
        /// <exception cref="ArgumentException">Exception when manufacturing date is older than 5 years.</exception>
        public Vehicle(string brand, string model, int manufacturingDate)
        {
            Id = Guid.NewGuid();
            Brand = brand;
            Model = model;
            ManufacturingDate = manufacturingDate;
            Avaible = true;
        }

        /// <summary>
        /// Gets or sets vehicle Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets vehicle brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets vehicle model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets manufacturing date.
        /// </summary>
        public int ManufacturingDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets if the vehicle is avaibled.
        /// </summary>
        public bool Avaible { get; set; }

        /// <summary>
        /// Gets or sets client Id.
        /// </summary>
        public Guid? ClientId { get; set; }

        /// <summary>
        /// Rent Vehicle.
        /// </summary>
        /// <param name="clientId">Client id.</param>
        /// <exception cref="InvalidOperationException">Exception when the vehicle isn't avaibled for renting.</exception>
        public void Rent(Guid clientId)
        {
            if (!Avaible)
            {
                throw new InvalidOperationException("The vehicle isn't avaibled for renting");
            }

            Avaible = false;
            ClientId = clientId;
        }

        /// <summary>
        /// Return vehicle.
        /// </summary>
        /// <exception cref="InvalidOperationException">Exception when the vehicle isn't rented.</exception>
        public void Return()
        {
            if (!Avaible)
            {
                throw new InvalidOperationException("The vehicle isn't rented");
            }

            Avaible = true;
            ClientId = null;
        }
    }
}
