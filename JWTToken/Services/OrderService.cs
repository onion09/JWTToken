using JWTToken.Model.DBModel;
using JWTToken.Model.OrderResponse;
using JWTToken.Model.ViewModel;
using JWTToken.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace JWTToken.Services
{
    public class OrderService
    {
        private readonly OrderDbContext _dbContext;
        public OrderService(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public OrderInfo GetOrderInfo(int orderId)
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
        public UserResponse GetUserById(int userId)
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
        public async Task<OrderResponse> GetOrderResponse(int orderId)
        {
            var tasklist = GetOrderItemsAsync(orderId);
            var orderItemList = new List<OrderItemResponse>();
            var orderResponse = new OrderResponse();
            orderResponse.OrderId = orderId;            
            var list = await tasklist;
            var taskorder = GetOrderById(orderId);
            foreach (var item in list)
            {
                orderItemList.Add(new OrderItemResponse { ItemName = item.Item.ItemName, Quantity = item.Quantity });
            }
            var order = await taskorder;
            orderResponse.OrderItemResponseList = orderItemList;
            orderResponse.Time = order.OrderTime;
            orderResponse.TotalPrice = order.TotalPrice;
                
            
            return orderResponse;
        }

        private async Task<List<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            var list = await _dbContext.OrderItems.Include(oi => oi.Item).Where(oi => oi.OrderId == orderId).ToListAsync();
            return list;
        }
        private async Task<Order> GetOrderById(int orderId)
        {
            var order = await _dbContext.Orders
               .Where(o => o.OrderId == orderId).FirstOrDefaultAsync();
            return order;
        }
    }
}
