using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Combo")]
    public class Combo
    {
        [Key]
        public Guid ComboId { get; set; }
        public string ComboName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public Guid ItemId { get; set; }
        public Item? Item { get; set; }
        
        public ICollection<ComboDetail>? ComboDetails { get; set; }
    }
}
