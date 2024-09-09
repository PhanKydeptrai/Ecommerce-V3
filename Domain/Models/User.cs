using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("User")]
    public class User
    {
        // Yc nhập khi đăng kí
        [Key]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đầy đủ Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(50, ErrorMessage = "Email không được vượt quá 50 ký tự.")]
        public string Email { get; set; }
        //tối thiểu 8 ký tự

        [MinLength(8, ErrorMessage = "Mật khẩu phải có tối thiểu 8 ký tự")]
        public string Password { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0")]
        [MinLength(10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự")]
        [MaxLength(10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đầy đủ ngày tháng năm sinh")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string LastName { get; set; }

        // Bổ sung thêm cho account không yc nhập khi đăng kí
        public string? Address { get; set; }
        public string? Image { get; set; }

        // có bộ phận quản lí riêng (admin)
        public string Status { get; set; }
        public string Role { get; set; }
        public ICollection<Order>? Orders { get; set; }

    }
}
