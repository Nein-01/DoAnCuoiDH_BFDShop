using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("ParentCategory")]

    public class ParentCategory
    {
        public ParentCategory()
        {
            ChildCategory = new HashSet<ChildCategory>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //category id
        public int parentcategory_id { get; set; }
        [Required]
        //category name
        [StringLength(100, ErrorMessage = "Danh mục không được qua 100 ký tự")]
        public string name { get; set; }
        //slug
        [Required]
        [StringLength(109)]
        public string slug { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Ảnh 1 không được quá 500 ký tự")]
        public string image { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Ảnh 2 không được quá 500 ký tự")]
        public string image2 { get; set; }
        //category description
        [StringLength(100, ErrorMessage = "Mô tả không được quá 100 ký tự")]
        public string category_description { get; set; }
        public DateTime create_at { get; set; }
        //update at
        public DateTime update_at { get; set; }
        [StringLength(100)]
        public string create_by { get; set; }
        [StringLength(100)]
        public string update_by { get; set; }
        //update by
        [StringLength(1)]
        public string status { get; set; }
        public virtual ICollection<ChildCategory> ChildCategory { get; set; }
    }
}