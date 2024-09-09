using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_Models
{
    public class ComboDetailCreationDto
    {
        public Guid? ComboDetailId { get; set; }
        public Guid ComboId { get; set; }
        public Guid ProductId { get; set; }
        public int Quanlity { get; set; }

    }
}
