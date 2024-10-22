namespace Task.Application.DTOs
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public double Weight { get; set; }
        public string District { get; set; }
        public string StartDeliveryTime { get; set; }
        public string EndDeliveryTime { get; set; }
    }
}
