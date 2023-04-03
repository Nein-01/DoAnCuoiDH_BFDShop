using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Genre")]
    public class Genre
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Genre()
        {
            Products = new HashSet<Product>();
        }
        //Genre ID
        [Key]
        public int genre_id { get; set; }
        [Required]
        public int parent_genre_id { get; set; }
        //Genre Name
        [Required(ErrorMessage = "Vui lòng nhập tên thể loại")]
        [StringLength(100, ErrorMessage = "Tên thể loại tối đa 100 ký tự", MinimumLength = 1)] 
        public string genre_name { get; set; }
        [StringLength(109)]
        public string slug { get; set; }
        [StringLength(500)]
        //Genre Image
        public string genre_image { get; set; }
        //Decription
        [StringLength(200, ErrorMessage = "Mô tả không được quá 200 ký tự")]
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
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
        [ForeignKey("parent_genre_id")]
        public virtual ParentGenres ParentGenres { get; set; }

    }
}