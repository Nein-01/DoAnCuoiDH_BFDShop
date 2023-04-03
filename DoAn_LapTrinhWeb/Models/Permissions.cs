using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace DoAn_LapTrinhWeb.Models
{
    [Table("Permissions")]
    public class Permissions
    {
        //Permission ID
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int permission_id { get; set; }
        //Permission Name
        [Required(ErrorMessage = "Nhập chức năng cho phép sử dụng")]
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự", MinimumLength = 1)]
        public string permission_name { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }
        public virtual ICollection<RolesPermissions> RolesPermissions { get; set; }
    }
}