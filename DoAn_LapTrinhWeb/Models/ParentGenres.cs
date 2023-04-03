using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("ParentGenres")]
    public class ParentGenres
    {
        public ParentGenres()
        {
            Genres = new HashSet<Genre>();
        }
        //tag id
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên thể loại")]
        [StringLength(150, ErrorMessage = "Tên thể loại tối đa 150 ký tự", MinimumLength = 1)]
        public string name { get; set; }
        //slug
        [StringLength(159)]
        public string slug { get; set; }
        //slug
        [StringLength(20, ErrorMessage = "Icon Không được quá 20 ký tự")]
        public string icon { get; set; }
        //
        [StringLength(500, ErrorMessage = "Ảnh Không được quá 500 ký tự")]
        public string image { get; set; }
        //Decription
        [StringLength(200, ErrorMessage = "Mô tả Không được quá 200 ký tự")]
        public string description { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
    }
}