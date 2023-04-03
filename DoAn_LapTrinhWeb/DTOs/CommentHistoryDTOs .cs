using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class CommentHistoryDTOs
    {
        public string account_name { get; set; }
        //=>
        public int comment_id { get; set; }
        //=>
        public int comment_account_id { get; set; }
        //=>
        public int comment_news_id { get; set; }
        //=>
        public string comment_content { get; set; }
        //=>

        public string comment_status { get; set; }
        //=>
        public int reply_id { get; set; }
        //=>
        public int reply_acc_id { get; set; }
        //=>
        public string reply_status { get; set; }
        //=>
        public int reply_comment_id { get; set; }
        //=>
        public string reply_comment_content { get; set; }
        //=>
        public int news_id { get; set; }
        //=>
        public string news_title { get; set; }
        //=>
        public string news_slug { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime comment_at { get; set; }
        //=>
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime reply_at { get; set; }
        //=>
    }
}