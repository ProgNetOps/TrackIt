using AutoMapper;
using TrackIt.Domain;
using TrackIt.Presentation.Models.DTO;

namespace TrackIt.Presentation.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BTS, BTSDTO>().ReverseMap();
            CreateMap<State, StateDTO>().ReverseMap();
            CreateMap<Zone, ZoneDTO>().ReverseMap();
            CreateMap<NetworkSwitch, NetworkSwitchDTO>().ReverseMap();
            CreateMap<IPPoP, IPPoPDTO>().ReverseMap();
            CreateMap<Circuit, CircuitDTO>().ReverseMap();
            CreateMap<Client, ClientDTO>().ReverseMap();
            CreateMap<Service, ServiceDTO>().ReverseMap();
            CreateMap<CircuitState,CircuitStateDTO>().ReverseMap();
        }
    }
}
