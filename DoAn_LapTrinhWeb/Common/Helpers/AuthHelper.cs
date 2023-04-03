using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DoAn_LapTrinhWeb.Common.Helpers
{
	public static class AuthHelper
	{
		public static string GetName(this IIdentity identity)
		{
			return identity.GetUserData().Name;
		}
		public static string GetAvartar(this IIdentity identity)
		{
			return identity.GetUserData().Avatar;
		}
		public static int GetUserId(this IIdentity identity)
		{
			return identity.GetUserData().UserId;
		}
		public static string GetEmail(this IIdentity identity)
		{
			return identity.GetUserData().Email;
		}
		public static string GetPhoneNumber(this IIdentity identity)
		{
			return identity.GetUserData().PhoneNumber;
		}
		public static int GetRole(this IIdentity identity)
		{
			return identity.GetUserData().RoleCode;
		}
		public static string GetRoleName(this IIdentity identity)
		{
			return identity.GetUserData().Rolename;
		}
		public static bool Permiss_Create(this IIdentity identity)
		{
			return identity.GetUserData().Permission_create;
		}
		public static bool Permiss_Read(this IIdentity identity)
		{
			return identity.GetUserData().Permission_read;
		}
		public static bool Permiss_Update(this IIdentity identity)
		{
			return identity.GetUserData().Permission_update;
		}
		public static bool Permiss_Modify(this IIdentity identity)
		{
			return identity.GetUserData().Permission_modify;
		}
		public static bool Permiss_Delete(this IIdentity identity)
		{
			return identity.GetUserData().Permission_delete;
		}
		public static bool Permiss_Access(this IIdentity identity)
		{
			return identity.GetUserData().Permission_access;
		}
		public static LoggedUserData GetUserData(this IIdentity identity)
		{
			var jsonUserData = HttpContext.Current.User.Identity.Name;
			var userData = JsonConvert.DeserializeObject<LoggedUserData>(jsonUserData);
			return userData;
		}
	}
}