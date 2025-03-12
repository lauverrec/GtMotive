using GtMotive.Estimate.Microservice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Interfaces
{
    /// <summary>
    /// Vehicle repository.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Add vehicle method.
        /// </summary>
        /// <param name="vehicle">Vehicle.</param>
        /// <returns>void.</returns>
        Task AddAsync(Vehicle vehicle);

        /// <summary>
        /// Get all vehicle method.
        /// </summary>
        /// <returns>Enumerable of vehicles.</returns>
        Task<IEnumerable<Vehicle>> GetAllAsync();

        /// <summary>
        /// Get vehicle by Id method.
        /// </summary>
        /// <param name="id">Vehicle Id.</param>
        /// <returns>Vehicle.</returns>
        Task<Vehicle> GetByIdAsync(Guid id);

        /// <summary>
        /// Get avaibles vehicle method.
        /// </summary>
        /// <returns>Enumerable of vehicles.</returns>
        Task<IEnumerable<Vehicle>> GetAvaiblesVehiclesAsync();

        /// <summary>
        /// Update vehicle.
        /// </summary>
        /// <param name="vehicle">Vehicle.</param>
        /// <returns>Void.</returns>
        Task UpdateAsync(Vehicle vehicle);
    }
}
