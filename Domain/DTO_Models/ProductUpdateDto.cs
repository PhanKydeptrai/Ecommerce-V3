using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO_Models
{
    public class ProductUpdateDto
    {
        
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ địa chỉ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng của mặt hàng")]
        public int Quanlity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đơn vị cho mặt hàng")]
        public string Unit { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập size cho mặt hàng")]
        public string Size { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá cho mặt hàng")]
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public string? Image { get; set; } //Tạm thời null 
        
        public Guid ItemId { get; set; }
    }
}
