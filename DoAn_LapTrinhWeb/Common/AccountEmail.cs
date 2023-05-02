using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Common
{
    public class AccountEmail
    {
        //để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
        public const string Host = "smtp.gmail.com"; //tên máy chủ dùng gmail thì là  Host = "smtp.gmail.com" || smtp server riêng thì Host = "ten_smtp_server" 
        //tài khoản Email
        public const string UserEmail = "vanquangkrk@gmail.com";
        public const string UserEmailSupport = "vanquangkrk@gmail.com";
        //Mật khẩu google App-password
        public const string Password = "ixtmaykjxctnlrep";
        //Tên Email hiển thị khi gửi
        public const string Name = "VQuang";
    }
}