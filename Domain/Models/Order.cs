using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedOfDate { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }
        public User? User { get; set; }
        public Recomment? Recomment { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}
