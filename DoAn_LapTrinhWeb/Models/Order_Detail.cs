using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    public partial class Order_Detail
    {
        //Product ID
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int product_id { get; set; }
        //Genre ID
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int genre_id { get; set; }
        //Order ID
        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int order_id { get; set; }
        //Price
        public double price { get; set; }
        //Status
        [StringLength(1)]
        public string status { get; set; }
        [StringLength(20)]
        public string discount_code { get; set; }
        //Quantity
        public int quantity { get; set; }
        //Create By
        [Required]
        [StringLength(100)]
        public string create_by { get; set; }
        //Create At
        public DateTime create_at { get; set; }
        //Update By
        [Required]
        [StringLength(100)]
        public string update_by { get; set; }
        //Update At
        public DateTime update_at { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
