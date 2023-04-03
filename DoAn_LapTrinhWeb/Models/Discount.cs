using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Discount")]
    public class Discount
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Discount()
        {
            Products = new HashSet<Product>();
        }
        //Discount ID
        [Key] public int disscount_id { get; set; }
        //tên chương trình giảm giá
        [Required(ErrorMessage ="Nhập tên chương trình giảm giá")]
        [StringLength(200, ErrorMessage = "Tên chương trình giảm giá tối đa 200 ký tự", MinimumLength = 1)]
         public string discount_name { get; set; }
        //Discount Start
        [Required(ErrorMessage = "Nhập ngày bắt đầu chương trình giảm giá")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime discount_start { get; set; }
        //Discount End
        [Required(ErrorMessage = "Nhập ngày kết thúc chương trình giảm giá")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime discount_end { get; set; }
        //Discount Price
        [Required(ErrorMessage = "Nhập mức giảm")]
        public double discount_price { get; set; }
        //
        public double discount_max { get; set; }
        //Discount Code
        [StringLength(20, ErrorMessage = "Mã giảm giá không được quá 20 ký tự")]
        public string discounts_code { get; set; }
        //Discount Type ("1" giảm giá sản phẩm,"2" code giảm giá)
        [Required(ErrorMessage = "Chọn loại giảm giá")]
        public int discounts_type { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Create By
        [Required] [StringLength(100)]
        public string create_by { get; set; }
        //Status
        [StringLength(1)] public string status { get; set; }
        [StringLength(1)] public string discount_global { get; set; }
        //Quantity
        [StringLength(10)] public string quantity { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Upadte At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}