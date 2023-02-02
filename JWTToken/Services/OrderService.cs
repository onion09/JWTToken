using JWTToken.Model.DBModel;
using JWTToken.Model.OrderResponse;
using JWTToken.Model.ViewModel;
using JWTToken.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace JWTToken.Services
{
    public class OrderService
    {
        private readonly OrderDbContext _dbContext;
        public OrderService(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public OrderInfo GetOrderResponse(int orderId)
        {
            var order = _dbContext.Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .Include(o => o.User).ThenInclude(u=>u.UserDetail)
                .Where(o => o.OrderId == orderId).FirstOrDefault();
            if (order == null) return null;
            var orderInfo = new OrderInfo();
            orderInfo.serviceStatus = new ServiceStatus { Success = true };
            orderInfo.orderResponse = new OrderResponse
            {
                OrderId = order.OrderId,
                Time = order.OrderTime,
                TotalPrice = order.TotalPrice,
                OrderItemResponseList = order.OrderItems.Select(oi => new OrderItemResponse
                {
                    ItemName = oi.Item.ItemName,
                    Quantity = oi.Quantity,
                }).ToList(),
            };
            orderInfo.userResponse = new UserResponse
            {
                username = order.User.UserName,
                FirstName = order.User.UserDetail.FirstName,
                LastName = order.User.UserDetail.LastName,
                Email = order.User.UserDetail.Email,
            };
            return orderInfo;
        }
        private UserResponse GetUserById(int userId)
        {
            var userdetail = new UserResponse();

            var user = _dbContext.Users.Include(q => q.UserDetail).Where(q => q.UserID == userId).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            userdetail.UserId = userId;
            userdetail.Email = user.UserDetail.Email;
            userdetail.FirstName = user.UserDetail.FirstName;
            userdetail.LastName = user.UserDetail.LastName;
            userdetail.username = user.UserName;


            return userdetail;
        }


    }
}
