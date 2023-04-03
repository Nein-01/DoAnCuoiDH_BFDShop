using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Models
{
    public class Register
    {

        [Required(ErrorMessage = "Chọn Tỉnh/Thành Phố")]
        public int province_id { get; set; }
        [Required(ErrorMessage = "Quận/Huyện")]
        public int district_id { get; set; }
        [Required(ErrorMessage = "Chọn Phường xã")]
        public int ward_id { get; set; }

        public int account_id { get; set; }
        //Password
        [Required(ErrorMessage = "Nhập mật khẩu")]
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$", 
        ErrorMessage = "Mật khẩu tổi thiếu 8 ký tự bao gồm: chữ thường, chữ hoa, chữ số và 1 ký tự đặc biệt")]
        public string password { get; set; }
        //Email
        [Required(ErrorMessage = "Nhập Email")]
        [StringLength(100, ErrorMessage = "Email tối thiểu 6 ký tự", MinimumLength = 1)]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        public string Email { get; set; }
        //Request Code
        [StringLength(100)]
        public string Requestcode { get; set; }
        //Roles
        [Required(ErrorMessage = "Chọn quyền")]
        [StringLength(1)] public string Role { get; set; }
        //Name
        [Required(ErrorMessage = "Nhập họ tên")]
        [StringLength(20, ErrorMessage = "Họ tên tối đa 20 ký tự",MinimumLength = 1)]
        public string Name { get; set; }
        //Phone Number
        [StringLength(11)]
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [MinLength(10, ErrorMessage = "Số diện thoại phải đủ 10 chữ số")]
        [RegularExpression("^(0|84)([0-9]{9})$",
        ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 hoặc 84, chứa ký tự số từ (0 -> 9) và đủ 10 chữ số")]
        public string Phone { get; set; }
        //Avatar
        [StringLength(500, ErrorMessage = "Ảnh đại diện không được quá 500 ký tự")]
        public string Avatar { get; set; }
        //Address
        [Required(ErrorMessage = "Nhập địa chỉ")]
        [StringLength(100, ErrorMessage = "Địa chỉ không được quá 100 ký tự", MinimumLength = 1)]
        public string Addres_1 { get; set; }
        //Date Of Birth
        [Required(ErrorMessage = "Nhập Ngày sinh")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime Dateofbirth { get; set; }
        //Gender
        [StringLength(1)]

        [Required(ErrorMessage = "Chọn giới tính")]
        public string Gender{ get; set; }
        //Create By
        [StringLength(100)]
        public string create_by { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Upadte By
        [StringLength(100)]
        public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }


    }
}