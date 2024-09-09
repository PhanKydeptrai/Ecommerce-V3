using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ tên danh mục.")]
        public string CategoryName { get; set; }
        public string? Image { get; set; } //Tạm thời cho phép null
        public string Status { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
