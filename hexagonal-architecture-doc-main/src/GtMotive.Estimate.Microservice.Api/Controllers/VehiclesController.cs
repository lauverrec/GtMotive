using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GtMotive.Estimate.Microservice.Api.Dtos;
using GtMotive.Estimate.Microservice.ApplicationCore.Services;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IAppLogger<Vehicle> _logger;
        private readonly ITelemetry _telemetry;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleService vehicleService, IAppLogger<Vehicle> logger, ITelemetry telemetry, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _logger = logger;
            _telemetry = telemetry;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleDto vehicleDto)
        {
            try
            {
                var vehicle = _mapper.Map<Vehicle>(vehicleDto);

                vehicle = await _vehicleService.CreateVehicleAsync(vehicle);

                _telemetry.TrackEvent("VehicleCreated", new Dictionary<string, string>
                {
                    { "vehicleId", vehicle.Id.ToString() }
                });

                _logger.LogInformation($"Vehicle with id {vehicle.Id} have been created");

                return Ok(_mapper.Map<VehicleDto>(vehicle));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(new InvalidOperationException("Error creating vehicle"), null);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            var vehicles = await _vehicleService.ListAvaibleVehiclesAsync();
            var vehicle = vehicles.FirstOrDefault(v => v.Id == id);

            return vehicle == null ? NotFound() : Ok(_mapper.Map<VehicleDto>(vehicle));
        }

        [HttpGet]
        public async Task<IActionResult> ListAvaibleVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.ListAvaibleVehiclesAsync();
                return Ok(vehicles);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(new InvalidOperationException("Error listing vehicles"), null);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
