using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace DoAn_LapTrinhWeb.Models
{
    [Table("Roles")]
    public class Roles
    {
        public Roles()
        {
            Accounts = new HashSet<Account>();
        }
        //Role ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int role_id { get; set; }
        //Role Name
        [Required(ErrorMessage = "Nhập tên quyền")]
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự", MinimumLength = 1)]
        public string role_name { get; set; }
        //Create At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime create_at { get; set; }
        //Update At
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime update_at { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<RolesPermissions> RolesPermissions { get; set; }
    }
}