using AutoMapper;
using PetCare.Shared.DTOs;
using PetCare.Shared.Entities;

namespace PetCare.Server.Mappers
{
    public class VaccineProfile : Profile
    {
        public VaccineProfile()
        {
            CreateMap<Vaccine, VaccineDTO>().ReverseMap();
            CreateMap<UpcomingVaccine, UpcomingVaccineDTO>().ReverseMap();
        }
    }
}
