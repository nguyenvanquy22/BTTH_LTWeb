using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Lab2.Models
{
    //public class Student : Controller
    //{
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }
    //}
    public class Student
    {
        public int Id { get; set; }//Mã sinh viên

        [StringLength(100, MinimumLength = 4, ErrorMessage = "Số kí tự phải nằm trong khoảng từ 4 đến 100")]
        [Required(ErrorMessage = "Bạn chưa nhập tên")]
        public string? Name { get; set; } //Họ tên

        [Required(ErrorMessage = "Email bắt buộc phải được nhập hoặc phải có đuôi gmail.com")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@gmail+\.com")]
        public string? Email { get; set; } //Email

        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Mật khẩu phải chứa ít nhất một ký tự viết hoa, một ký tự viết thường, một chữ số và một ký tự đặc biệt.")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string? Password { get; set; }//Mật khẩu

        [Required]
        public Branch? Branch { get; set; }//Ngành học

        [Required(ErrorMessage = "Bạn chưa chọn giới tính")]
        public Gender? Gender { get; set; }//Giới tính
        public bool IsRegular { get; set; }//Hệ: true-chính qui, false-phi cq

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        public string? Address { get; set; }//Địa chỉ

        [Range(typeof(DateTime), "1/1/1963", "31/12/2005", ErrorMessage ="Ngày sinh không hợp lệ")]
        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không hợp lệ")]
        [Required(ErrorMessage = "Ngày sinh không hợp lệ")]
        public DateTime DateOfBorth { get; set; }//Ngày sinh

        [Range(0.0, 10.0, ErrorMessage = "Giá trị phải trong khoảng từ 0 đến 10")]
        [Required(ErrorMessage = "Bạn chưa nhập điểm")]
        public double? Point { get; set; }
    }

}
