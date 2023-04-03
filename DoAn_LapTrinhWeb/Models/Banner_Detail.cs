using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoAn_LapTrinhWeb.Models;
namespace DoAn_LapTrinhWeb.Model
{
    public class Banner_Detail
    {
        //Banner ID
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Chọn khuyến mãi")]
        public int banner_id { get; set; }
        //Product ID
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Chọn sản phẩm")]
        public int product_id { get; set; }
        //Genre ID
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int genre_id { get; set; }
        //ID
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Create At
        public DateTime create_at { get; set; }
        public virtual Banner Banner { get; set; }
        public virtual Product Product { get; set; }
    }
}