namespace DoAn_LapTrinhWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Warranty")]
    public partial class Warranty
    {
        [Key]
        public int warranty_id { get; set; }

        public int? order_id { get; set; }

        [StringLength(150)]
        public string broken_state { get; set; }

        [StringLength(150)]
        public string fixed_state { get; set; }

        public DateTime warranty_date { get; set; }

        public DateTime receive_date { get; set; }

        [StringLength(150)]
        public string addition_parts { get; set; }

        [StringLength(50)]
        public string note { get; set; }

        [Required]
        [StringLength(100)]
        public string create_by { get; set; }

        public DateTime create_at { get; set; }

        [Required]
        [StringLength(100)]
        public string update_by { get; set; }

        public DateTime update_at { get; set; }

        public int? status { get; set; }
        public int? product_id { get; set; }
    }
}
