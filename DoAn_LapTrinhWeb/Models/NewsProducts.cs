using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("NewsProducts")]
    public class NewsProducts
    {
        [Key]
        [Column(Order =0)]
        public int news_id { get; set; }
        [Key]
        [Column(Order = 1)]
        public int product_id { get; set; }
        [Required]
        public int genre_id { get; set; }
        public virtual News News { get; set; }
        public virtual Product Product { get; set; }
    }
}