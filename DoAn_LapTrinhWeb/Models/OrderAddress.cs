using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("OrderAddress")]
    public class OrderAddress
    {
        [Key] [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int order_address_id { get; set; }
        [StringLength(10)]
        public string order_address_phonenumber { get; set; }
        //số diện
        [StringLength(20)]
        public string order_address_username { get; set; }
        //tỉnh thành phố
        [StringLength(100)]
        public string order_adress_email { get; set; }
        //tỉnh, thành phố
        [StringLength(50)]
        public string order_adress_province { get; set; }
        //quận, huyện
        [StringLength(50)]
        public string order_adress_district { get; set; }
        //phường xã
        [StringLength(50)]
        public string order_adress_wards { get; set; }
        [StringLength(150)]
        public string order_address_content { get; set; }
        public int times_edit_adress { get; set; }
    }
}