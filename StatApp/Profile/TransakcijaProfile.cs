using AutoMapper;
using fixit.DTO;
using fixit.Models;

namespace fixit.Profiles
{
    public class TransakcijaProfile : Profile
    {
        public TransakcijaProfile()
        {
            CreateMap<Transakcija, TransakcijaDto>()
            .ForMember(dest => dest.Transakcija_id, opt => opt.MapFrom(src => src.Transakcija_id))
            .ForMember(dest => dest.Potrosnja, opt => opt.MapFrom(src => src.Potrosnja))
            .ForMember(dest => dest.Ime, opt => opt.MapFrom(src => src.Ime))
            .ForMember(dest => dest.Prezime, opt => opt.MapFrom(src => src.Prezime))
            .ForMember(dest => dest.Jmbg, opt => opt.MapFrom(src => src.Jmbg))
            .ForMember(dest => dest.Proizvod, opt => opt.MapFrom(src => src.Proizvod));
            CreateMap<TransakcijaDto, Transakcija>();
        }
    }
}