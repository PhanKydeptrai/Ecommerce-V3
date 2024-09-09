using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Item")]
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }
        public string Status { get; set; }
        //dùng để phân biệt product và combo
        public string ItemType { get; set; }
        public ICollection<OrderDetail>? OrderDetail { get; set; }
        public Combo? Combo { get; set; }
        public Product? Product { get; set; }
    }
}
