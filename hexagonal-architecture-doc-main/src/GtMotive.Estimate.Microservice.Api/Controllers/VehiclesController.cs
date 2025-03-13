using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Command;
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

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
        {
            try
            {
                var vehicleId = await _mediator.Send(command);
                return Ok(vehicleId);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvaibleVehicles()
        {
            _logger.LogInformation("Get avaible Vehicles");
            var vehicles = await _mediator.Send(new GetAvaiblesVehiclesQuery());
            return Ok(vehicles);
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentVehicle([FromBody] RentVehicleCommand command)
        {
            var result = await _mediator.Send(command);

            return !result
                ? BadRequest("Can't rent the vehicle. Verify if it is avaible or if the client had already rented a vehicle.")
                : Ok("Vehicle rent sucessfully.");
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnVehicle([FromBody] ReturnVehicleCommand command)
        {
            var result = await _mediator.Send(command);
            return !result ? BadRequest("Can't return vehicle") : Ok("Vehicle return sucessfully.");
        }
    }
}
