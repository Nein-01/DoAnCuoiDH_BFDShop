using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Account_Address")]
    public class Account_Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int account_address_id { get; set; }
        [Required]
        public int account_id { get; set; }
        [StringLength(10)]
        public string account_address_phonenumber { get; set; }
        //số diện
        [StringLength(20)]
        public string account_address_username { get; set; }
        [Required]
        //tỉnh thành phố
        public int  province_id { get; set; }
        [Required]
        //quận, huyện
        public int district_id { get; set; }
        //phường xã
        [Required]
        public int ward_id { get; set; }
        [StringLength(50)]
        //địa chỉ cụ thể
        public string account_address_content { get; set; }
        //đặt làm đia chỉ mặc định hay không
        public bool account_address_default { get; set; }
        public virtual Account Account { get; set; }
        public virtual Wards Wards { get; set; }
        public virtual Districts Districts { get; set; }
        public virtual Provinces Provinces { get; set; }
    }
}