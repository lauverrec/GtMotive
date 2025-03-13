using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces.Dtos;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Api.Mappers
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleDto>();
        }
    }
}
