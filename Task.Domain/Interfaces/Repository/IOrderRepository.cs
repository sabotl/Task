using Task.Domain.Entity;

namespace Task.Domain.Interfaces.Repository
{
    public interface IOrderRepository
    {
        System.Threading.Tasks.Task<IEnumerable<Order>> GetAllOrders();
        System.Threading.Tasks.Task AddOrder(Order order);
        System.Threading.Tasks.Task<IEnumerable<Order>> GetOrdersByDistrictAndTime(string district, DateTime startTime, DateTime endTime);
    }
}
