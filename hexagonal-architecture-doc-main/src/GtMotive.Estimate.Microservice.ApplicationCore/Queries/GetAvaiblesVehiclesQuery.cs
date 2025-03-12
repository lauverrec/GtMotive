using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Domain.Entities;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Queries
{
    /// <summary>
    /// Get avaibles vehicles query.
    /// </summary>
    public class GetAvaiblesVehiclesQuery : IRequest<IEnumerable<Vehicle>>
    {
    }
}
