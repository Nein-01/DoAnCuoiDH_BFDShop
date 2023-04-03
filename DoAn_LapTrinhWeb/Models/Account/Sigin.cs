using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Models
{

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
    public class Sigin
    {
        public int account_id { get; set; }
        //Email
        [Required(ErrorMessage = "Nhập email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //Password
        [Required(ErrorMessage = "Nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        //Roles
        [StringLength(1)] public string Role { get; set; }
        //Name
        public string Name { get; set; }
        //Avatar
        public string Avatar { get; set; }
    }
}   