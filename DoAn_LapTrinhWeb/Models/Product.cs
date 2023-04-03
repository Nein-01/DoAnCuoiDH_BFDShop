using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using DoAn_LapTrinhWeb.Model;


namespace DoAn_LapTrinhWeb.Models
{
    [Table("Product")]
    public class Product
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Banner_Detail = new HashSet<Banner_Detail>();
            Feedbacks = new HashSet<Feedback>();
            Order_Detail = new HashSet<Order_Detail>();
        }
        //Product ID
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key] [Column(Order = 0)] public int product_id { get; set; }
        //Genre ID
        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        public int genre_id { get; set; }
        //
        [ForeignKey("Taxes")]
        public int? taxes_id { get; set; }
        //Brand ID
        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int brand_id { get; set; }
        //Discount ID
        [Required(ErrorMessage = "Vui lòng chọn chương trình giảm giá")]
        public int disscount_id { get; set; }
        //Product Name
        [StringLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự", MinimumLength = 1)]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string product_name { get; set; }
        //Seo Title
        [StringLength(150, ErrorMessage = "Tiêu đề SEO tối đa 100 ký tự", MinimumLength = 1)]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề SEO")]
        public string title_seo { get; set; }
        //Seo Title
        [StringLength(159)]
        public string slug { get; set; }
        //Price
        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public double price { get; set; }
        //View
        public long view { get; set; }
        //Buy Turn
        public long buyturn { get; set; }
        //Quantity
        private string _quantity;
        [StringLength(10, ErrorMessage = "Số lượng không được quá 10000", MinimumLength = 1)]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public string quantity
        {
            get { return ((this._quantity != "" && this._quantity != null) ? this._quantity.Trim() : this._quantity); }
            set { this._quantity = (value == null) ? "" : value.Trim(); }
        }
        //Status
        [StringLength(1)] public string status { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Update By
        [StringLength(100)]
        public string updateby { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        //Image
        [StringLength(500, ErrorMessage = "Ảnh sản phẩm không được quá 500 ký tự", MinimumLength = 1)]
        public string image { get; set; }
        //Decription
        public string description { get; set; }
        //Specification
        public string specification { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Banner_Detail> Banner_Detail { get; set; }
        public virtual Taxes Taxes { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Discount Discount { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual Genre Genre { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Detail { get; set; }
        public virtual ICollection<NewsProducts> NewsProducts { get; set; }
    }
}