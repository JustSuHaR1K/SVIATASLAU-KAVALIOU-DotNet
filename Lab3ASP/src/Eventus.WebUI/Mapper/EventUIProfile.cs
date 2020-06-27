using AutoMapper;
using BusinessLogic.Models;
using Eventus.WebUI.ViewModels;

namespace Eventus.WebUI.Mapper
{
    public class EventUIProfile : Profile
    {
        public EventUIProfile()
        {
            CreateMap<EventViewModel, Event>().ReverseMap();
            CreateMap<MasterViewModel, Master>().ReverseMap();
            CreateMap<OrderViewModel, Order>().ReverseMap();
        }
    }
}