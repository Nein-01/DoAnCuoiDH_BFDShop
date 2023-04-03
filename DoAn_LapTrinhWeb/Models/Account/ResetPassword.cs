using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.Models
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "Nhập mật khẩu mới", AllowEmptyStrings = false)]
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$",
        ErrorMessage = "Mật khẩu tổi thiếu 8 ký tự bao gồm: chữ thường, chữ hoa, chữ số và 1 ký tự đặc biệt")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu khác nhận", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác thực không trùng nhau")]

        public string ConfirmPassword { get; set; }

        [Required] public string ResetCode { get; set; }
    }
}