using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Provinces")]
    public class Provinces
    {
        public Provinces()
        {
            Districts = new HashSet<Districts>();
        }
        //tag id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int province_id { get; set; }
        [Required(ErrorMessage = "Nhập tên Tỉnh/Thành Phố")]
        //tag name
        [StringLength(50)]
        public string province_name { get; set; }

        [Required]
        [StringLength(20)]
        public string type { get; set; }
        public virtual ICollection<Districts> Districts { get; set; }
        public virtual ICollection<Account_Address> Account_Addresses { get; set; }
    }
}