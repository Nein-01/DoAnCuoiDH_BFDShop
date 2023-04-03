using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Payment")]
    public class Payment
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Payment()
        {
            Orders = new HashSet<Order>();
        }
        //Payment ID
        [Key] public int payment_id { get; set; }
        //Payment Name
        [Required] [StringLength(100, ErrorMessage = "tên phương thức thanh toán không được quá 100 ký tự", MinimumLength = 1)] public string payment_name { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //tỉ giá đối hoái tính theo usd
        [StringLength(30)] public string Exchange_rates { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}