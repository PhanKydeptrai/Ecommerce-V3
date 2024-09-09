using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("ComboDetail")]
    public class ComboDetail
    {
        [Key]
        public Guid ComboDetailId { get; set; }
        public Guid ComboId { get; set; }
        public Guid ProductId { get; set; }
        public int Quanlity { get; set; }
        public Combo? Combo { get; set; }
        public Product? Product { get; set; }
    }
}
