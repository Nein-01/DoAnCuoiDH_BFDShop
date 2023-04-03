using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace DoAn_LapTrinhWeb.Model
{
    [Table("Banner")]
    public class Banner
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Banner()
        {
            Banner_Detail = new HashSet<Banner_Detail>();
        }
        //Banner ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int banner_id { get; set; }
        //Banner Name
        [Required(ErrorMessage = "Nhập tên khuyến mãi")]
        [StringLength(200, ErrorMessage = "Tên khuyến mãi không được quá 200 ký tự", MinimumLength = 1)]
        public string banner_name { get; set; }
        //slug
        [StringLength(209)]
        public string slug { get; set; }
        //Banner Start
        [Required(ErrorMessage = "Nhập ngày bắt đầu")]
        public DateTime banner_start { get; set; }
        //Banner End
        [Required(ErrorMessage = "Nhập ngày kết thúc")]
        public DateTime banner_end { get; set; }
        //Banner Image
        [StringLength(100, ErrorMessage = "Mô tả không được quá 100 ký tự", MinimumLength = 1)]
        public string description { get; set; }
        //Image Thumbnail
        [Required(ErrorMessage = "Thêm ảnh thumbnail")]
        [StringLength(500, ErrorMessage = "Ảnh thumbnail không được quá 500 ký tự")]
        public string image_thumbnail { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Create At
        public DateTime create_at { get; set; }
        //Banner Type ("1"ngang - trên,"2" ngang - dưới,3"dọc - 2 bên")
        public int banner_type { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Update At
        public DateTime update_at { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Banner_Detail> Banner_Detail { get; set; }
    }
}