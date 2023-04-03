using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Feedback")]
    public partial class Feedback
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Feedback()
        {
            Feedback_Image = new HashSet<Feedback_Image>();
        }
        //Feedback ID
        [Column(Order = 0)]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int feedback_id { get; set; }

        //Acount ID
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int account_id { get; set; }
        //Product ID
        public int product_id { get; set; }
        //Genre ID
        public int genre_id { get; set; }
        //
        public int parent_feedback_id { get; set; }
        //Discount ID
        [StringLength(200, ErrorMessage = "Nội dung đánh giá không được quá 200 ký tự", MinimumLength = 1)]
        public string description { get; set; }
        //Rate Star
        [Required]
        public int rate_star { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Create By
        [Required]
        [StringLength(100)]
        public string create_by { get; set; }
        //Status
        [StringLength(1)]
        public string status { get; set; }
        //Update By
        [Required]
        [StringLength(100)]
        public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        public virtual Account Account { get; set; }
        public virtual Product Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback_Image> Feedback_Image { get; set; }
    }
}
