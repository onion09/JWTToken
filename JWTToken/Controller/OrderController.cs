using JWTToken.Exceptions;
using JWTToken.Model.DBModel;
using JWTToken.Model.OrderResponse;
using JWTToken.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.AccessControl;
using static JWTToken.Services.UseerServices;

namespace JWTToken.Controller
{
    [ApiController]
    public class OrderController: ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly UserService _user;

        public OrderController(OrderService orderService, UserService user, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _user = user;
            _logger = logger;
        }
        [HttpGet("/user/{userId}/order/{orderId}")]
        public ActionResult UserOrderResponse(int userId, int orderId)
        {
            var stopwatch = Stopwatch.StartNew();
            var userOrderInfo = _orderService.GetOrderResponse(orderId);
            if(userOrderInfo == null)
            {
                throw new UserNotFoundException($"order {orderId} not found");
            }
            stopwatch.Stop();
            _logger.LogInformation($"GetOrder({orderId}) executed in {stopwatch.ElapsedMilliseconds} ms");
            return Ok(userOrderInfo);
        }
    }
}
