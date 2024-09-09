using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailId { get; set; }
        public int Quanlity { get; set; }
        public decimal Price { get; set; }
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        //Khoá ngoại bảng Item
        public Item? Item { get; set; }
        //Khoá ngoại bảng Order
        public Order? Order { get; set; }
        
    }
}
