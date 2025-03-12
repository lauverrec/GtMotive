using GtMotive.Estimate.Microservice.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Interfaces
{
    /// <summary>
    /// Client repository.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Add client method.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <returns>Void.</returns>
        Task AddAsync(Client client);

        /// <summary>
        /// Get Client by Id.
        /// </summary>
        /// <param name="id">Id of client.</param>
        /// <returns>Client.</returns>
        Task<Client> GetByIdAsync(Guid id);

        /// <summary>
        /// Update client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <returns>void.</returns>
        Task UpdateAsync(Client client);
    }
}
