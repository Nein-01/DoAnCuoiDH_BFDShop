using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.DTOs
{
 
    public class NewsDTO
    {
        public List<NewstagsCheckbox> Tags { get; set; }
        public List<NewsProductsCheckbox> Products { get; set; }
        //news id
        public int news_id { get; set; }
        public int countproducts { get; set; }
        public int counttags { get; set; }
        //author
        public string author { get; set; }
        public int account_id { get; set; }
        public int account_post { get; set; }
        public int account_comment { get; set; }
        public string account_avatar { get; set; }
        //news category id
        [Required(ErrorMessage = "Chọn danh mục")]
        public int child_category_id { get; set; }
        //category name
        public string  parent_category_name { get; set; }
        //news title
        [Required(ErrorMessage = "Nhập tiêu đề bài viết")]
        [StringLength(500, ErrorMessage = "Tiêu đề bài viết không được quá 500 ký tự")]
        public string news_title { get; set; }
        //meta title
        [Required(ErrorMessage = "Nhập tiêu đề SEO")]
        [StringLength(150,ErrorMessage = "Tiêu đề SEO không được quá 500 ký tự")]
        public string meta_title { get; set; }
        //slug
        [StringLength(159)]
        public string news_slug { get; set; }
        //news content
        [Required(ErrorMessage = "Nhập nội dung bài viết")]
        public string news_content { get; set; }
        public int news_comment { get; set; }
        //viewcount
        public int ViewCount { get; set; }
        //image 
        [Required(ErrorMessage = "Chọn ảnh")]
        [StringLength(500, ErrorMessage = "Ảnh 1 không được quá 500 ký tự")]
        public string image { get; set; }
        //image2 
        [Required(ErrorMessage = "Chọn ảnh")]
        [StringLength(500, ErrorMessage = "Ảnh 2 không được quá 500 ký tự")]
        public string image2 { get; set; }
        //create at
        public DateTime create_at { get; set; }
        //update at
        public DateTime update_at { get; set; }
        //update by
        [StringLength(100)]
        public string update_by { get; set; }
        //status
        [StringLength(1)]
        public string status { get; set; }
        //tag name
        public int tag_id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Tên tag không được quá 100 ký tự")]
        //tag name
        public string tag_name { get; set; }
        //tag slug
        public string tag_slug { get; set; }
        //=>
        public string product_image { get; set; }
        //=>
        public string product_name { get; set; }
        //=>
        public string product_slug { get; set; }
        //=>
        public int count_product_post { get; set; }
        //=>
        public double product_price { get; set; }
        public class NewstagsCheckbox
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool Checked { get; set; }
        }
        public class NewsProductsCheckbox 
        {
            public int id { get; set; }
            public int discount_id { get; set; }
            public int genre_id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public bool Checked { get; set; }
        }

    }
}