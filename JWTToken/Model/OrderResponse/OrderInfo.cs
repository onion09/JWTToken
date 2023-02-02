
namespace JWTToken.Model.OrderResponse
{
    public class OrderInfo
    {
        public ServiceStatus serviceStatus { get; set; }
        public OrderResponse orderResponse { get; set; }
        public UserResponse userResponse { get; set; }
    }
}
