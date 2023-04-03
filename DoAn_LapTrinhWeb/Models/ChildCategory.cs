using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("ChildCategory")]

    public class ChildCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //id
        public int childcategory_id { get; set; }
        //category id
        [Required]
        public int parentcategory_id { get; set; }
        //category name
        [Required(ErrorMessage = "Nhập tên danh mục")]
        [StringLength(100, ErrorMessage = "Địa chỉ không được quá 100 ký tự")]
        public string name { get; set; }
        //slug
        [Required]
        [StringLength(150)]
        public string slug { get; set; }
        //image
        [Required]
        [StringLength(500, ErrorMessage = "Ảnh 1 không được quá 500 ký tự")]
        public string image { get; set; }
        //image2
        [Required]
        [StringLength(500, ErrorMessage = "Ảnh 2 không được quá 500 ký tự")]
        public string image2 { get; set; }
        //category description
        [StringLength(100, ErrorMessage = "Địa chỉ không được quá 100 ký tự")]
        public string description { get; set; }
        //create at
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //update at
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        //create by
        [StringLength(100)]
        public string create_by { get; set; }
        //update by
        [StringLength(100)]
        public string update_by { get; set; }
        //status
        [StringLength(1)]
        public string status { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ParentCategory ParentCategory { get; set; }
    }
}