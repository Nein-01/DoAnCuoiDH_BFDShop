using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class ProductDTOs
    {
        //=>Product Name
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string product_name { get; set; }
        //Seo Title
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề SEO")]
        public string seo_title { get; set; }
        //=>
        public int parent_genre_id{ get; set; }
        //=>
        public string genre_name { get; set; }
        //=>
        public string genre_slug { get; set; }
        //=>
        public string parent_genre_name { get; set; }
        //=>
        public string parent_genre_slug { get; set; }
        //=>
        public string brand_name { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        //=>
        public int brand_id { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        //=>
        public int genre_id { get; set; }
        //=>
        [Required(ErrorMessage = "Vui lòng chọn VAT")]
        public int taxes_id { get; set; }
        //=>
        public string taxes_name{ get; set; }
        //=>
        public int taxes_value { get; set; }
        //=>
        public int product_id { get; set; }
        //=>
        public string slug { get; set; }
        //=>
        public string specification { get; set; }
        //=>
        public string description { get; set; }
        //=>
        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        public double price { get; set; }
        //=>
        public string price_format { get; set; }
        //=>
        public string price2 { get; set; }
        //=>
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public string quantity { get; set; }
        //=>
        public int product_img_id { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public string Image { get; set; }
        //=>
        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public string image_1 { get; set; }
        //=>
        public string image_2 { get; set; }
        //=>
        public string image_3 { get; set; }
        //=>
        public string image_4 { get; set; }
        //=>
        public string image_5 { get; set; }
        //=>
        public string create_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //=>
        public string update_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        //=>
        public string status { get; set; }
        //=>
        public string product_img_status { get; set; }
        //=>
        public long buyturn { get; set; }
        //=>
        public long view { get; set; }
        //=>
        public double discount_price { get; set; }
        //=>
        [Required(ErrorMessage = "Vui lòng chọn chương trình giảm giá")]
        public int discount_id { get; set; }
        //=>
        public string discount_name { get; set; }
        //=>
        public int count_Banner_detail { get; set; }
        //=>
        public int count_Order_detail { get; set; }
        //=>
         public int count_feedback { get; set; }
        //=>
        public DateTime discount_start { get; set; }
        //=>
        public string discount_status { get; set; }
        //=>
        public DateTime discount_end { get; set; }
        //=>
        public string genre_status { get; set; }
        //=>
        public string brand_status { get; set; }
        //=>
        public string brand_slug { get; set; }
        //=>
        public int orderdetail_product_id { get; set; }
        //=>
        public int order_account_id { get; set; }
        //=>
        public int countfeedback_product { get; set; }
        //=>
        public int count_post { get; set; }
        //=>
    }
}