using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class OrderHistoryDTOs
    {
        //=>
        public int order_id { get; set; }
        //=>
        public string order_status { get; set; }
        //=>
        public string account_id { get; set; }
        //=>
        public string Avatar { get; set; }
       //=>
        public string Name { get; set; }
        //=>
        public string Role { get; set; }
        //=>
        public string Email { get; set; }
        //=>
        public string Phone { get; set; }
        //=>
        public string Address { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime order_date { get; set; }
        //=>
        public string account_status { get; set; }
        //=>
        public int payment_id { get; set; }
        //=>
        public string payment_transaction { get; set; }
        //=>
        public string payment_name { get; set; }
        //=>
        public string delivery_name { get; set; }
        //=>
        public string product_name { get; set; }
        //=>
        public string discount_status { get; set; }
        //=>
        public int delivery_id { get; set; }
        //=>
        public string order_note { get; set; }
        //=>
        public double total_price { get; set; }
        //=>
        public int total_order { get; set; }
        //=>
        public double sum_total_order_price { get; set; }
        //=>
    }
}