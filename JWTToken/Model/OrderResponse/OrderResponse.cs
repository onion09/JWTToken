using JWTToken.Model.DBModel;

namespace JWTToken.Model.OrderResponse
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public DateTime Time { get; set; }
        public int TotalPrice { get; set; }
        public List<OrderItemResponse> OrderItemResponseList { get; set; }
        
    }
}
