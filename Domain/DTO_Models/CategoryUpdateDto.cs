using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_Models
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ tên danh mục.")]
        public string CategoryName { get; set; }
        public string? Image { get; set; } //Tạm thời cho phép null
        public string Status { get; set; }
    }
}
