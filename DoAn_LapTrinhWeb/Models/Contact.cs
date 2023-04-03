using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Contact")]
    public class Contact
    {
        //Contact ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int contact_id { get; set; }
        //Name
        [StringLength(50, ErrorMessage = "Họ tên không được quá 100 ký tự", MinimumLength = 1)]
        [Required(ErrorMessage = "Nhập họ tên")]
        public string name { get; set; }
        //Phone Number
        [StringLength(10)]
        [RegularExpression("^(0)([0-9]{9})$",
        ErrorMessage = "Số điện thoại phải bắt đầu bằng 0, chứa ký tự số từ (0 -> 9) và đủ 10 chữ số")]
        public string phone { get; set; }
        //Email
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        [Required(ErrorMessage = "Nhập Email")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự", MinimumLength = 1)]
        public string email { get; set; }
        //Content
        [StringLength(200, ErrorMessage = "Nội dung gửi không được quá 200 ký tự", MinimumLength = 1)]
        [Required(ErrorMessage = "Nhập nội dung")]
        public string content { get; set; }
        [StringLength(500, ErrorMessage = "Ảnh không được quá 500 ký tự", MinimumLength = 1)]
        //Image
        public string image { get; set; }

        public string reply { get; set; }
        [Required]
        //Flag
        public int flag { get; set; }
        //Status
        [Required] [StringLength(1)] public string status { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        //cái dùng cho lúc nhập form thôi
    }
}