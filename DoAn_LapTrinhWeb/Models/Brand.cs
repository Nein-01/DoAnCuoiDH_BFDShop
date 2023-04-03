using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace DoAn_LapTrinhWeb.Models
{
    [Table("Brand")]
    public class Brand
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Brand()
        {
            Products = new HashSet<Product>();
        }
        //Brand ID
        [Key] public int brand_id { get; set; }
        //Brand Name
        [Required(ErrorMessage = "Nhập tên thương hiệu")]
        [StringLength(100, ErrorMessage = "Tên thương hiệu tối đa 100 ký tự", MinimumLength = 1)]
        public string brand_name { get; set; }
        //Brand Image
        [StringLength(500, ErrorMessage = "Ảnh thương hiệu không được quá 500 ký tự", MinimumLength = 1)]
        public string brand_image { get; set; }
        [StringLength(109)]
        public string slug { get; set; }
        //Decription
        [StringLength(200, ErrorMessage = "Mô tả không được quá 200 ký tự", MinimumLength = 1)]
        public string description { get; set; }
        //Web directory
        [StringLength(200, ErrorMessage = "đường dẫn website tối đa 200 ký tự", MinimumLength = 1)]
        public string Web_directory { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? update_at { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}