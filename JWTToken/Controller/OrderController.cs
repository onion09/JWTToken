using JWTToken.Exceptions;
using JWTToken.Model.DBModel;
using JWTToken.Model.OrderResponse;
using JWTToken.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            var userOrderInfo = _orderService.GetOrderInfo(orderId);
            if(userOrderInfo == null)
            {
                throw new UserNotFoundException($"order {orderId} not found");
            }
            stopwatch.Stop();
            _logger.LogInformation($"GetOrder orderid: {orderId} userId:{userId}executed in {stopwatch.ElapsedMilliseconds} ms");
            return Ok(userOrderInfo);
        }
        [HttpGet("/user/async/{userId}/order/{orderId}")]
        public async Task<IActionResult> UserOrderResponseAsync(int userId, int orderId)
        {
            var stopwatch = Stopwatch.StartNew();
            Task<OrderResponse> response = _orderService.GetOrderResponse(orderId);
            var serviceStatus = new ServiceStatus { Success = true };
            var user = _user.GetUserResponseById(userId);
            var orderResponse = await response;
            var orderInfo = new OrderInfo { userResponse=user,serviceStatus=serviceStatus, orderResponse= orderResponse };
            stopwatch.Stop();

            _logger.LogInformation($"async GetOrder orderid: {orderId} userId:{userId}executed in {stopwatch.ElapsedMilliseconds} ms");
            return Ok(orderInfo);

        }
    }
}
