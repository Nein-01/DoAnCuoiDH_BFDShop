using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Common
{
	/// <summary>
	/// Chứa thông tin account sau khi đăng nhập
	/// </summary>
	public class LoggedUserData
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public bool Permission_create { get; set; }
		public bool Permission_read { get; set; }
		public bool Permission_update { get; set; }
		public bool Permission_modify { get; set; }
		public bool Permission_delete { get; set; }
		public bool Permission_access { get; set; }
		public int RoleCode { get; set; }
		public string Rolename { get; set; }
		public string Avatar { get; set; }
		public string PhoneNumber { get; set; }
	}
}