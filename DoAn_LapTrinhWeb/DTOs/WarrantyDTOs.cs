using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class WarrantyDTOs
    {
        //=>
        public int warranty_id { get; set; }
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
        public string broken_state { get; set; }
        //=>
        public string fixed_state { get; set; }
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
        public DateTime warranty_date { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime receive_date { get; set; }
        //=>
        public int status { get; set; }
        //=>
        public string product_name { get; set; }
        //=>
        public string product_image { get; set; }
        //=>
        public int warranty_time { get; set; }
        //=>
        public string note { get; set; }
        //=>
        public string addition_parts { get; set; }
        //=>
        public int account_id { get; set; }
        //=>
        public int product_id { get; set; }
    }
}