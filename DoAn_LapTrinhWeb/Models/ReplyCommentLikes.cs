using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("ReplyCommentLikes")]
    public class ReplyCommentLikes
    {
        //reply like id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reply_like_id { get; set; }
        //reply comment id
        public int reply_comment_id { get; set; }
        //account id
        public int account_id { get; set; }
        //reply comment like
        [StringLength(1)]
        public string reply_like { get; set; }
        //create at
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        public virtual Reply_Comments Reply_Comment { get; set; }
        public virtual Account Accounts { get; set; }
    }
}