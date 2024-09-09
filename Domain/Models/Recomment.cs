using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Recomment")]
    public class Recomment
    {
        [Key]
        public Guid CommentId { get; set; }

        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        [StringLength(255, ErrorMessage = "Đã vượt quá số lượng ký tự. Bình luận chỉ giới hạn 255 ký tự")]
        public string Comment { get; set; }
        public string? Image { get; set; } //Khách có thể đăng ảnh hoặc không.
        public Order? Order { get; set; }
    }
}
