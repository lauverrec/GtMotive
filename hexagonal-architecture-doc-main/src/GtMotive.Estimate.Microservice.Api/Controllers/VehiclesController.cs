using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GtMotive.Estimate.Microservice.Api.Dtos;
using GtMotive.Estimate.Microservice.Api.Filters;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Command;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    [ServiceFilter(typeof(BusinessExceptionFilter))]
    public class VehiclesController : ControllerBase
    {
        private readonly IAppLogger<Vehicle> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public VehiclesController(IAppLogger<Vehicle> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
        {
            try
            {
                var vehicle = await _mediator.Send(command);
                _logger.LogInformation($"Vehicle with id {vehicle.Id} has created sucessfully.");
                return Ok(_mapper.Map<VehicleDto>(vehicle));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvaibleVehicles()
        {
            var vehicles = await _mediator.Send(new GetAvaiblesVehiclesQuery());

            _logger.LogInformation($"There are {vehicles.Count()} vehicles avaibles.");

            return Ok(_mapper.Map<List<VehicleDto>>(vehicles));
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentVehicle([FromBody] RentVehicleCommand command)
        {
            if (command == null)
            {
                throw new InvalidOperationException("Command is null");
            }

            _logger.LogInformation($"Rent vehicle with id {command.VehicleId}");

            var result = await _mediator.Send(command);

            return !result
                ? BadRequest("Can't rent the vehicle. Verify if it is avaible or if the client had already rented a vehicle.")
                : Ok("Vehicle rent sucessfully.");
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnVehicle([FromBody] ReturnVehicleCommand command)
        {
            if (command == null)
            {
                throw new InvalidOperationException("Command is null");
            }

            _logger.LogInformation($"Return vehicle with id {command.VehicleId}");

            var result = await _mediator.Send(command);

            return !result ? BadRequest("Can't return vehicle") : Ok("Vehicle return sucessfully.");
        }
    }
}
