using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Account")]
    public class Account
    {
        //Account_ID
        [Key]
        public int account_id { get; set; }
        //role id FK
        public int? role_id { get; set; }
        //Password
        [StringLength(100)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]))(?=.*[#$^+=!*()@%&]).{8,}$", 
        ErrorMessage = "Mật khẩu tổi thiếu 8 ký tự bao gồm: chữ thường, chữ hoa, chữ số và 1 ký tự đặc biệt")]
        public string password { get; set; }
        //Email
        [StringLength(100, ErrorMessage = "Email tối thiểu  ký tự", MinimumLength = 1)]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        public string Email { get; set; }
        //Request Code
        [StringLength(100)]
        public string Requestcode { get; set; }
        //Name
        [Required(ErrorMessage = "Nhập họ tên")]
        [StringLength(20, ErrorMessage = "Họ tên tối đa 20 ký tự",MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        //Phone Number
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [StringLength(10)]
        [RegularExpression("^(0)([0-9]{9})$",
        ErrorMessage = "Số điện thoại phải bắt đầu bằng 0, chứa ký tự số từ (0 -> 9) và đủ 10 chữ số")]
        public string Phone { get; set; }
        //Avatar
        [StringLength(500, ErrorMessage = "Ảnh đại diện không được quá 500 ký tự")]
        public string Avatar { get; set; }
        //Date Of Birth
        [Required(ErrorMessage = "Nhập Ngày sinh")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM-dd-yyyy}")]
        [DataType(DataType.Date)]
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
        //expiredat
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime expired_at { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        public virtual Roles Roles { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Account_Address> Addresses { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<NewsComments> NewsComments { get; set; }
        public virtual ICollection<Reply_Comments> Reply_Comments { get; set; }
        public virtual ICollection<CommentLikes> CommentLikes { get; set; }
        public virtual ICollection<ReplyCommentLikes> ReplyCommentLikes { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}