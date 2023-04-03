using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Product_Image")]
    public class Product_Image
    {
        //Product Image ID
        [Key][Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int product_image_id { get; set; }
        //Product ID
        [Required]
        public int product_id { get; set; }
        //Genre ID
        [Required]
        public int genre_id { get; set; }
        //Image 1
        [StringLength(500, ErrorMessage = "Ảnh sản phẩm không được quá 100 ký tự", MinimumLength = 1)]
        public string image_1 { get; set; }
        //Image 2
        [StringLength(500, ErrorMessage = "Ảnh sản phẩm không được quá 100 ký tự", MinimumLength = 1)]
        public string image_2 { get; set; }
        //Image 3
        [StringLength(500, ErrorMessage = "Ảnh sản phẩm không được quá 100 ký tự", MinimumLength = 1)]
        public string image_3 { get; set; }
        //Image 4
        [StringLength(500, ErrorMessage = "Ảnh sản phẩm không được quá 100 ký tự", MinimumLength = 1)]
        public string image_4 { get; set; }
        //Image 5
        [StringLength(500, ErrorMessage = "Ảnh sản phẩm không được quá 100 ký tự", MinimumLength = 1)]
        public string image_5 { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Create By
        [StringLength(100)]
        public string create_by { get; set; }
        //Update By
        [StringLength(100)]
        public string update_by { get; set; }
        //Create At
        public DateTime create_at { get; set; }
        //Upate At
        public DateTime update_at { get; set; }
        public virtual Product Product { get; set; }
    }
}