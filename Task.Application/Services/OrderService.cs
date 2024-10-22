using AutoMapper;
using Task.Application.DTOs;
using Task.Application.Interfaces;
using Task.Domain.Entity;
using Task.Domain.Interfaces.Repository;

namespace Task.Application.Services
{
    public class OrderService: Interfaces.IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async System.Threading.Tasks.Task AddOrder(CreateOrderDTO order)
        {
            await _orderRepository.AddOrder(_mapper.Map<Order>(order));
        }
        public async System.Threading.Tasks.Task<IEnumerable<OrderDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<OrderDTO>>(await _orderRepository.GetAllOrders());
        }
        public async Task<IEnumerable<OrderDTO>> FilterOrders(FilterDTO filterDTO)
        {
            var orders = await _orderRepository.GetOrdersByDistrictAndTime(filterDTO.Distinct, filterDTO.startDate, filterDTO.endnDate);
            if (orders == null || !orders.Any())
            {
                return Enumerable.Empty<OrderDTO>();
            }

            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
    }
}
