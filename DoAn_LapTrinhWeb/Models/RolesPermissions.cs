using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace DoAn_LapTrinhWeb.Models
{
    [Table("RolesPermissions")]
    public class RolesPermissions
    {
        [Key]
        [Column(Order = 0)]
        //Role ID
        public int role_id { get; set; }
        [Key]
        [Column(Order = 1)]
        //Permission ID
        public int permission_id { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual Permissions Permissions { get; set; }
    }
}