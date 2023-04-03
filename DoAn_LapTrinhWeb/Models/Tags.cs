using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Tags")]
    public class Tags
    {
        //tag id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tag_id { get; set; }
        [Required(ErrorMessage = "Nhập tên tag")]
        //tag name
        [StringLength(100, ErrorMessage = "Tên tag không được quá 100 ký tự", MinimumLength = 1)]
        public string tag_name { get; set; }
        //slug
        [Required]
        [StringLength(100)]
        public string slug { get; set; }
        public virtual ICollection<NewsTags> NewsTags { get; set; }
    }
}