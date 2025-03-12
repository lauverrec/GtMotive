using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Queries;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IAppLogger<Vehicle> _logger;
        private readonly IMediator _mediator;

        public VehiclesController(IAppLogger<Vehicle> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvaibleVehicles()
        {
            _logger.LogInformation("Get avaible Vehicles");
            var vehicles = await _mediator.Send(new GetAvaiblesVehiclesQuery());
            return Ok(vehicles);
        }
    }
}
