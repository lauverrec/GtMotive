using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Services
{
    /// <summary>
    /// Vehicle service.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Create vehicle.
        /// </summary>
        /// <param name="vehicle">Vehicle.</param>
        /// <returns><see cref="Vehicle"/>.</returns>
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);

        /// <summary>
        /// List avaibled vehivles.
        /// </summary>
        /// <returns>Enumerable of vehicle.</returns>
        Task<IEnumerable<Vehicle>> ListAvaibleVehiclesAsync();

        /// <summary>
        /// Rent vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle Id.</param>
        /// <param name="clientId">Client Id.</param>
        /// <returns>void.</returns>
        Task RentVehicleAsync(Guid vehicleId, Guid clientId);

        /// <summary>
        /// Return vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle Id.</param>
        /// <param name="clientId">Client Id.</param>
        /// <returns>void.</returns>
        Task ReturnVehicleAsync(Guid vehicleId, Guid clientId);
    }

    /// <summary>
    /// Vehicle service class.
    /// </summary>
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        /// <param name="clientRepository">Client repository.</param>
        public VehicleService(IVehicleRepository vehicleRepository, IClientRepository clientRepository)
        {
            _vehicleRepository = vehicleRepository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Create vehicle.
        /// </summary>
        /// <param name="vehicle">Vehicle.</param>
        /// <returns><see cref="Vehicle"/>.</returns>
        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new InvalidOperationException("Vehicle is null");
            }

            var res = new Vehicle(vehicle.Brand, vehicle.Model, vehicle.ManufacturingDate);
            await _vehicleRepository.AddAsync(res);

            return res;
        }

        /// <summary>
        /// List avaibled vehivles.
        /// </summary>
        /// <returns>Enumerable of vehicle.</returns>
        public async Task<IEnumerable<Vehicle>> ListAvaibleVehiclesAsync()
        {
            return await _vehicleRepository.GetAvaiblesVehiclesAsync();
        }

        /// <summary>
        /// Rent vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle Id.</param>
        /// <param name="clientId">Client Id.</param>
        /// <returns>void.</returns>
        public async Task RentVehicleAsync(Guid vehicleId, Guid clientId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);

            if (vehicle != null)
            {
                var client = await _clientRepository.GetByIdAsync(clientId);

                if (client != null)
                {
                    client.RentVehicle(vehicle);

                    await _vehicleRepository.UpdateAsync(vehicle);
                    await _clientRepository.UpdateAsync(client);
                }
                else
                {
                    throw new InvalidOperationException("client not found");
                }
            }
            else
            {
                throw new InvalidOperationException("Vehicle not found");
            }
        }

        /// <summary>
        /// Return vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle Id.</param>
        /// <param name="clientId">Client Id.</param>
        /// <returns>void.</returns>
        public async Task ReturnVehicleAsync(Guid vehicleId, Guid clientId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);

            if (vehicle != null)
            {
                var client = await _clientRepository.GetByIdAsync(clientId);

                if (client != null)
                {
                    client.ReturnVehicle(vehicle);

                    await _vehicleRepository.UpdateAsync(vehicle);
                    await _clientRepository.UpdateAsync(client);
                }
                else
                {
                    throw new InvalidOperationException("client not found");
                }
            }
            else
            {
                throw new InvalidOperationException("Vehicle not found");
            }
        }
    }
}
