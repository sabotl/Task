using Task.Application.DTOs;
using Task.Domain.Entity;

namespace Task.Application.Interfaces
{
    public interface IOrderService
    {
        System.Threading.Tasks.Task AddOrder(CreateOrderDTO order);
        System.Threading.Tasks.Task<IEnumerable<OrderDTO>> GetAll();
        System.Threading.Tasks.Task<IEnumerable<OrderDTO>> FilterOrders(FilterDTO filterDTO);
    }
}
