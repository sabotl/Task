using Microsoft.AspNetCore.Mvc;
using Task.Application.DTOs;
using Task.Application.Interfaces;

namespace Task.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILoggerService _loggerService;

        public OrderController(IOrderService orderService, ILoggerService loggerService)
        {
            _orderService = orderService;
            _loggerService = loggerService;
        }

        private string UserIp => HttpContext?.Connection.RemoteIpAddress?.ToString();

        [HttpGet("getall")]
        public async Task<IEnumerable<OrderDTO>?> GetAll()
        {
            try
            {

                var orders = await _orderService.GetAll();

                _loggerService.Log($"Successfully retrieved {orders?.Count()} orders.", UserIp, Domain.Enums.LogLevel.Info);

                return orders;

            }catch(Exception ex)
            {
                _loggerService.Log($"Error occurred while retrieving orders: {ex.Message}", UserIp, Domain.Enums.LogLevel.Error);
                return Enumerable.Empty<OrderDTO>();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderDTO orderDto)
        {
            if (orderDto == null)
                return BadRequest("Order data is null.");

            try
            {
                await _orderService.AddOrder(orderDto);

                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();

                _loggerService.Log($"Order create", userIp, Domain.Enums.LogLevel.Info);
                return Ok("Order added successfully.");
            }
            catch (Exception ex)
            {
                _loggerService.Log($"Error occurred while retrieving orders: {ex.Message}", UserIp, Domain.Enums.LogLevel.Error);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("filter")]
        public async Task<IActionResult> FilterOrders([FromQuery] FilterDTO filterDTO)
        {
            if (string.IsNullOrWhiteSpace(filterDTO.Distinct))
                return BadRequest("District cannot be empty.");

            try
            {
                var filteredOrders = await _orderService.FilterOrders(filterDTO);

                _loggerService.Log($"Successfully filtered orders for district '{filterDTO.Distinct}' with {filteredOrders?.Count()} results.", UserIp, Domain.Enums.LogLevel.Info);

                return Ok(filteredOrders);
            }
            catch (Exception ex)
            {
                _loggerService.Log($"Error occurred while retrieving orders: {ex.Message}", UserIp, Domain.Enums.LogLevel.Error);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
