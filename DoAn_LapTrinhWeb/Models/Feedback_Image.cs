using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DoAn_LapTrinhWeb.Model
{
    public class Feedback_Image
    {
        //Image ID
        [Key] public int image_id { get; set; }
        //Feedback ID
        public int feedback_id { get; set; }
        //Account ID
        public int account_id { get; set; }
        //Image
        [Column(TypeName = "text")] public string image { get; set; }
        //Create At
        public DateTime create_at { get; set; }
        //Create By
        [Required] [StringLength(20)] public string create_by { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Update By
        [Required] [StringLength(20)] public string update_by { get; set; }
        //Update At
        public DateTime? update_at { get; set; }
        public virtual Feedback Feedback { get; set; }
     
        [NotMapped]
        public HttpPostedFileBase[] UploadImageMultiple { get; set; }
    }
}