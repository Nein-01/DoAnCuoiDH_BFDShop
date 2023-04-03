using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class ChekcoutOrderDTO
    {
        public int payment_id { get; set; }
        
        public string payment_transaction { get; set; }
        public string checkout_username { get; set; }
        public string checkout_address_province { get; set; }
        public string checkout_address_district { get; set; }
        public string checkout_address_ward { get; set; }
        public string checkout_address { get; set; }
        public string checkout_address_content { get; set; }
        public string checkout_phone_number { get; set; }
        public string checkout_email { get; set; }
        public int account_address_id { get; set; }
        public double total_price { get; set; }
    }
}