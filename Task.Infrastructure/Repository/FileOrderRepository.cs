using Microsoft.Extensions.Options;
using System.Text.Json;
using Task.Domain.Entity;
using Task.Domain.Interfaces.Repository;
using Task.Infrastructure.Options;

namespace Task.Infrastructure.Repository
{
    public class FileOrderRepository : IOrderRepository
    {
        private readonly string _filePath;
        public FileOrderRepository(IOptions<FileOrderRepositoryOptions> options)
        {
            _filePath = options.Value.FilePath;
        }

        public async System.Threading.Tasks.Task AddOrder(Order order)
        {
            var orderJson = JsonSerializer.Serialize(order);
            await File.AppendAllTextAsync(_filePath, orderJson + Environment.NewLine);
        }

        public async System.Threading.Tasks.Task<IEnumerable<Order>> GetOrdersByDistrictAndTime(string district, DateTime startTime, DateTime endTime)
        {
            var orders = await GetAllOrders();
            return orders.Where(o => o.District == district && o.StartDeliveryTime >= startTime && o.EndDeliveryTime <= endTime);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Order>();
            }

            var orders = new List<Order>();

            try
            {
                var lines = await File.ReadAllLinesAsync(_filePath);
                foreach (var line in lines)
                {
                    var order = JsonSerializer.Deserialize<Order>(line);
                    if (order != null)
                    {
                        orders.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading orders from file: {ex.Message}");
            }

            return orders;
        }

    }
}
