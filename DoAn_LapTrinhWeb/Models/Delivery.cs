using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Delivery")]
    public class Delivery
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Delivery()
        {
            Orders = new HashSet<Order>();
        }
        //Delivery ID
        [Key] public int delivery_id { get; set; }
        //Delivery Nam
        [Required]
        [StringLength(100, ErrorMessage = "Tên đơn vị vận chuyển không được quá 100 ký tự", MinimumLength = 1)] 
        public string delivery_name { get; set; }
        //Price
        [Column(TypeName = "money")] public decimal price { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}