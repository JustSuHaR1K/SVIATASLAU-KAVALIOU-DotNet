using AutoMapper;
using BusinessLogic.Models;
using Eventus.DAL.Models;

namespace BusinessLogic.Services.Mapper
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDto, Event>().ReverseMap();
            CreateMap<ClientDto, Client>().ReverseMap();
            CreateMap<MasterDto, Master>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}