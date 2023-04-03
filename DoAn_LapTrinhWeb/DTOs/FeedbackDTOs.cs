using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class FeedbackDTOs
    {
        //=>
        public int feedback_id { get; set; }
        //=>
        public int account_id { get; set; }
        //=>
        public string Name { get; set; }
        //=>
        public string User_Email { get; set; }
        //=>
        public int product_id { get; set; }
        //=>
        public string status { get; set; }
        //=>
        public string product_slug { get; set; }
        //=>
        public string product_name { get; set; }
        //=>
        public int genre_id { get; set; }
        //=>
        public int discount_id { get; set; }
        //=>
        public string description { get; set; }
        //=>
        public int rating_star { get; set; }
        //=>
        public string Image { get; set; }
        //=>
        public string reply { get; set; }
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
    }
}