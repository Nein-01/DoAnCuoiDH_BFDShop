using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace DoAn_LapTrinhWeb.Models
{
    public class Taxes
    {
        public Taxes()
        {
            Products = new HashSet<Product>();
        }
        //VAT ID
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int taxes_id { get; set; }
        //VAT Name
        [Required(ErrorMessage = "Nhập tên loại VAT")]
        [StringLength(50, ErrorMessage = "Tên VAT tối đa 50", MinimumLength = 1)]
        public string taxes_name { get; set; }
        //phần trăm VAT
        [Required(ErrorMessage = "Nhập tên loại VAT")]
        public int taxes_value { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}