using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTToken.Model.DBModel
{
    [Table("OrderItem")]
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
    }
}
