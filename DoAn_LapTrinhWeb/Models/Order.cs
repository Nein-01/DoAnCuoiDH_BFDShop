using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Order")]
    public class Order
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Detail = new HashSet<Order_Detail>();
        }
        //Order ID
        [Key] public int order_id { get; set; }
        //order_address id
        [Required]
        public int order_address_id { get; set; }
        //Payment ID
        public int payment_id { get; set; }
        //Delivery ID
        public int delivery_id { get; set; }
        //Order Date
        public DateTime oder_date { get; set; }
        //Account ID
        public int account_id { get; set; }
        //transaction
        [StringLength(1)]
        public string payment_transaction { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        [StringLength(200)]
        //Order Note
        public string order_note { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Total Price
        public double total { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        public virtual Account Account { get; set; }
        public virtual Delivery Delivery { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Detail { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual OrderAddress OrderAddress { get; set; }

        internal object Sum(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        internal object Sum(double total)
        {
            throw new NotImplementedException();
        }
    }
}