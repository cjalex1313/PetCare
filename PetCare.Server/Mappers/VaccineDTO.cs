using AutoMapper;
using PetCare.Shared.DTOs;
using PetCare.Shared.Entities;

namespace PetCare.Server.Mappers
{
    public class VaccineProfile : Profile
    {
        public VaccineProfile()
        {
            CreateMap<Vaccine, VaccineDto>().ReverseMap();
            CreateMap<UpcomingVaccine, UpcomingVaccineDto>().ReverseMap();
        }
    }
}
