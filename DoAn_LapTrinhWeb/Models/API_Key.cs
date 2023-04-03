using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("API_Key")]
    public class API_Key
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(100, ErrorMessage = "Tên API không quá 100 ký tự", MinimumLength = 1)]
        public string api_name { get; set; }
        //
        [StringLength(300, ErrorMessage = "Mô tả không quá 300 ký tự", MinimumLength = 1)]
        public string api_decription { get; set; }
        //
        [StringLength(500, ErrorMessage = "Client_ID không quá 500 ký tự", MinimumLength = 1)]
        public string client_id{ get; set; }
        //
        [StringLength(500, ErrorMessage = "Client_Secret không quá 500 ký tự", MinimumLength = 1)]
        public string client_secret { get; set; }
        //
        [StringLength(200, ErrorMessage = "Đường dẫn trả về không quá 200 ký tự", MinimumLength = 1)]
        public string Return_Url { get; set; }
        public bool active { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
    }
}