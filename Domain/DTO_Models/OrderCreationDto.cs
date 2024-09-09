using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_Models
{
    public class OrderCreationDto
    {
        public Guid? OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedOfDate { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }
    }
}
