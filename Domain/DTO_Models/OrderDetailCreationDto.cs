using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_Models
{
    public class OrderDetailCreationDto
    {
        public Guid? OrderDetailId { get; set; }
        public int Quanlity { get; set; }
        public decimal Price { get; set; }
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
    }
}
