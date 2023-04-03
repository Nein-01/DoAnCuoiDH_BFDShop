using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class OrderDTOs
    {
        //=>
        public int order_id { get; set; }
        //=>
        public string Name { get; set; }
        //=>
        public string Email { get; set; }
        //=>
        public string Phone { get; set; }
        //=>
        public string Address { get; set; }
        //=>
        public string create_by { get; set; }
        //=>
        public int total_quantity { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //=>
        public string update_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime order_date { get; set; }
        //=>
        public string status { get; set; }
        //=>
        public int payment_id { get; set; }
        //=>
        public string payment_name { get; set; }
        //=>
        public string delivery_name { get; set; }
        //=>
        public string product_name { get; set; }
        //=>
        public string discount_status { get; set; }
        //=>
        public int discount_type { get; set; }
        //=>
        public int delivery_id { get; set; }
        //=>
        public string order_note { get; set; }
        //=>
        public string payment_transaction { get; set; }
        //=>
        public int order_address_id { get; set; }
        //=>
        [Required(ErrorMessage ="Nhập họ tên")]
        public string order_address_username { get; set; }
        //=>
        [Required(ErrorMessage = "Nhập địa chỉ")]
        public string order_address_content { get; set; }
        //=>
        [Required(ErrorMessage = "Nhập Phường/xã")]
        public string order_address_ward { get; set; }
        //=>
        [Required(ErrorMessage = "Nhập Quận/huyện")]
        public string order_address_district { get; set; }
        //=>
        [Required(ErrorMessage = "Nhập Tỉnh/Thành phố")]
        public string order_address_province { get; set; }
        //=>
        [Required(ErrorMessage = "Nhập số điện thoại")]
        public string order_address_phonenumber { get; set; }
        //=>
        public int order_address_times_edit { get; set; }
        //=>
        public double price { get; set; }
        //=>
        public double discount_price { get; set; }
        //=>
        public double discount_max { get; set; }
        //=>
        public double temporary { get; set; }
        //=>
        public double total_price { get; set; }
        //=>
        public int account_id { get; set; }

    

    }
}