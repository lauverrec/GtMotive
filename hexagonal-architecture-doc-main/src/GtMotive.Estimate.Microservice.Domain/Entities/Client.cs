using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Client class.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="name">client's name.</param>
        public Client(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        /// <summary>
        /// Gets id of client.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets client's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets id of rented vehicle.
        /// </summary>
        public Guid? RentedVehicleId { get; set; }

        /// <summary>
        /// Rent vehicle method.
        /// </summary>
        /// <param name="vehicle">Vehicle for renting.</param>
        /// <exception cref="InvalidOperationException">Exception when the operation isn't valid because this vehicle has already rented.</exception>
        public void RentVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new InvalidOperationException("The vehicle is null");
            }

            if (RentedVehicleId.HasValue)
            {
                throw new InvalidOperationException("The client has already rented a vehicle");
            }

            vehicle.Rent(Id);
            RentedVehicleId = vehicle.Id;
        }

        /// <summary>
        /// Return a rented vehicle.
        /// </summary>
        /// <param name="vehicle">Rented vehicle.</param>
        /// <exception cref="InvalidOperationException">Exception when the operation isn't valid because this vehicle hasn't rented yet.</exception>
        public void ReturnVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new InvalidOperationException("The vehicle is null");
            }

            if (!RentedVehicleId.HasValue)
            {
                throw new InvalidOperationException("The client hasn't rented the vehicle");
            }

            vehicle.Return();
            RentedVehicleId = null;
        }
    }
}
