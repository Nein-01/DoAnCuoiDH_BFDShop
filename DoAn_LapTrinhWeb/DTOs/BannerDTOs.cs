using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class BannerDTOs
    {
        //=>
        public int banner_id { get; set; }
        //=>
        public int banner_detail_id { get; set; }
        //=>
        public string banner_name { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime banner_update_at { get; set; }
        //=>
        public string bannerdetail_create_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime bannerdetail_create_at { get; set; }
        //=>
        public string bannerdetail_update_by { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime bannerdetail_update_at { get; set; }
        //=>
        public int genre_id { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime banner_start { get; set; }
        //=>
        public DateTime banner_end { get; set; }
        //=>
        public string image { get; set; }
        //=>
        public string image_thumbnail { get; set; }
        //=>
        public string description { get; set; }
        //=>
        public string banner_status { get; set; }
        //=>
        public string banner_slug { get; set; }
        //=>
        public string bannerdetail_status { get; set; }
        //=>
        public int discount_id { get; set; }
        //=>
        public DateTime discount_start { get; set; }
        //=>
        public DateTime discount_end { get; set; }
        //=>
        public string product_name { get; set; }
        //=>
        public string seo_title { get; set; }
        //=>
        public string product_img { get; set; }
        //=>
        public int product_type { get; set; }
        //=>
        public double price { get; set; }
        //=>
        public double discount_price { get; set; }
        //=>
        public string discount_status { get; set; }
        //=>
        public int product_id { get; set; }
        //=>
        public string product_slug { get; set; }
      
    }
}