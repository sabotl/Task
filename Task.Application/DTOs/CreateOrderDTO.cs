using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task.Application.DTOs
{
    public class CreateOrderDTO
    {
        public double Weight { get; set; }
        public string District { get; set; }
        public string StartDeliveryTime { get; set; }
        public string EndDeliveryTime { get; set; }
    }
}
