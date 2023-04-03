using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Reply_Comments")]
    public class Reply_Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reply_comment_id { get; set; }
        //comment id
        public int comment_id { get; set; }
        //account id
        public int account_id { get; set; }
        //reply comment content
        [StringLength(500, ErrorMessage = "Nội dung bình luận không được quá 500 ký tự")]
        public string reply_comment_content { get; set; }
        //create at
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        ///status
        [StringLength(1)]
        public string status { get; set; }
        public virtual Account Accounts { get; set; }
        public virtual NewsComments NewsComments { get; set; }
        public virtual ICollection<ReplyCommentLikes> ReplyCommentLikes { get; set; }
    }
}