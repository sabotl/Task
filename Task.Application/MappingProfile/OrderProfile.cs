using Task.Application.DTOs;
using Task.Domain.Entity;

namespace Task.Application.MappingProfile
{
    public class OrderProfile: BaseMapperProfile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.StartDeliveryTime, opt => opt.MapFrom(src => src.StartDeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(dest => dest.EndDeliveryTime, opt => opt.MapFrom(src => src.EndDeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.StartDeliveryTime, opt => opt.MapFrom(src => DateTime.Parse(src.StartDeliveryTime)))
                .ForMember(dest => dest.EndDeliveryTime, opt => opt.MapFrom(src => DateTime.Parse(src.EndDeliveryTime)));


            CreateMap<CreateOrderDTO, Order>();
        }
    }
}
