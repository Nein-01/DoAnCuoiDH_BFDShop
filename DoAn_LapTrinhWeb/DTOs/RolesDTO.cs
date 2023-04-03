using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.DTOs
{
 
    public class RolesDTO
    {
        public List<RolePermissCheckbox> RolePermissions { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public int count_account_role { get; set; }
        public int permiss_id { get; set; }
        public string permiss_name { get; set; }

        public class RolePermissCheckbox
        {
            public int role_id { get; set; }
            public int permiss_id { get; set; }
            public string permiss_name { get; set; }
            public bool Checked { get; set; }
        }
    }
}