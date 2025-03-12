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
        public Vehicle(string brand, string model, DateTime manufacturingDate)
        {
            if ((manufacturingDate - DateTime.Now).TotalDays / 365 > 5)
            {
                throw new ArgumentException("The manufacturing date can't be older that 5 years");
            }

            Id = Guid.NewGuid();
            Brand = brand;
            Model = model;
            ManufacturingDate = manufacturingDate;
            Avaible = true;
        }

        /// <summary>
        /// Gets vehicle Id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets vehicle brand.
        /// </summary>
        public string Brand { get; private set; }

        /// <summary>
        /// Gets vehicle model.
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// Gets manufacturing date.
        /// </summary>
        public DateTime ManufacturingDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether gets if the vehicle is avaibled.
        /// </summary>
        public bool Avaible { get; private set; }

        /// <summary>
        /// Gets client Id.
        /// </summary>
        public Guid? ClientId { get; private set; }

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
