using System.ComponentModel.DataAnnotations;

namespace Domain.DTO_Models
{
    public class CategoryCreationDto
    {
        public Guid? CategoryId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ tên danh mục.")]
        public string CategoryName { get; set; }
        public string? Image { get; set; } //Tạm thời cho phép null
        public string Status { get; set; }
        
    }
}
