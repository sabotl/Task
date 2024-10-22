
namespace Task.Domain.Entity
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public double Weight { get; set; }
        public string District { get; set; }
        public DateTime StartDeliveryTime { get; set; }
        public DateTime EndDeliveryTime { get; set; }
    }
}
