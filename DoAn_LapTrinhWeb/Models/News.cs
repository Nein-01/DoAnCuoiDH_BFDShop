using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("News")]
    public partial class News
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public News()
        {
            NewsComments = new HashSet<NewsComments>();
            StickyPosts = new HashSet<StickyPost>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int news_id { get; set; }
        //
        public int account_id { get; set; }
        //news category
        public int childcategory_id { get; set; }
        //news title
        [Required]
        [StringLength(500, ErrorMessage = "Tiêu đề không được quá 500 ký tự")]
        public string news_title { get; set; }
        //meta title
        [Required]
        [StringLength(150, ErrorMessage = "Tiêu đề SEO không được quá 150 ký tự")]
        public string meta_title { get; set; }
        //slug
        [Required]
        [StringLength(159)]
        public string slug { get; set; }
        //news content
        [Required]
        public string news_content { get; set; }
        //viewcount
        public int ViewCount { get; set; }
        //image 
        [Required]
        [StringLength(500)]
        public string image { get; set; }
        //image2 
        [StringLength(500)]
        [Required]
        public string image2 { get; set; }
        //create at
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //update at
        public DateTime update_at { get; set; }
        //update by
        [StringLength(100)]
        public string update_by { get; set; }
        //status
        [StringLength(1)]
        public string status { get; set; }
        public virtual ICollection<NewsComments> NewsComments { get; set; }
        public virtual Account Accounts { get; set; }
        public virtual ChildCategory ChildCategory { get; set; }

        public virtual ICollection<StickyPost> StickyPosts { get; set; }
        public virtual ICollection<NewsTags> NewsTags { get; set; }
        public virtual ICollection<NewsProducts> NewsProducts { get; set; }
    }
}
