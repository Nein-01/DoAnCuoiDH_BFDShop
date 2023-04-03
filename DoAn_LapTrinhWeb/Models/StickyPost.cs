using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("StickyPost")]
    public class StickyPost
    {
        //tag id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int news_id { get; set; }

        //Create At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime sticky_start { get; set; }
        //Create By
        [Required] [StringLength(100)] public string create_by { get; set; }
        //Update By
        [Required] [StringLength(100)] public string update_by { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime sticky_end { get; set; }
        public virtual News News { get; set; }
    }
}