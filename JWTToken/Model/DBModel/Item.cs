using System.ComponentModel.DataAnnotations.Schema;

namespace JWTToken.Model.DBModel
{
    [Table("Item")]
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
   
    }
}
