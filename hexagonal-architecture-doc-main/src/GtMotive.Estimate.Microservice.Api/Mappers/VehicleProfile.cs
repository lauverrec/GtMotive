using System;
using System.Globalization;
using AutoMapper;
using GtMotive.Estimate.Microservice.Api.Dtos;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Api.Mappers
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<VehicleDto, Vehicle>()
                .ConstructUsing(vehicle => new Vehicle(vehicle.Brand, vehicle.Model, DateTime.Parse(vehicle.ManufacturingDate, CultureInfo.InvariantCulture)));
            CreateMap<Vehicle, VehicleDto>();
        }
    }
}
