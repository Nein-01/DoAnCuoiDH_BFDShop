    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAn_LapTrinhWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //rút gọn link tìm kiếm sản phẩm
            routes.MapRoute(
              name: "search",
              url: "search",
             defaults: new { Controller = "Products", action = "SearchResult" }
           );
//------------------------- start rút gọn link chi tiết sản phẩm------------------
            //rút gọn link chi tiết sản phẩm phụ kiện
            routes.MapRoute(
              name: "products detail",
              url: "product/{slug}",
             defaults: new { Controller = "Products", action = "ProductDetail" }
           );
//------------------------- end rút gọn link chi tiết sản phẩm--------------------
//------------------------- start rút gọn link danh mục sản phẩm------------------
            //rút gọn link laptop
            routes.MapRoute(
              name: "danh muc san pham",
              url: "category/{slug}",
             defaults: new { Controller = "Products", action = "ListProduct" }
           );
            //rút gọn link laptop
            routes.MapRoute(
              name: "danh muc thuong hieu",
              url: "brand/{slug}",
             defaults: new { Controller = "Products", action = "ListProduct" }
           );
 //------------------------- end rút gọn link danh mục sản phẩm------------------
            //rút gọn link giỏ hàng
            routes.MapRoute(
                name: "cart",
                url: "cart",
                defaults: new { controller = "Cart", action = "ViewCart" }
            );
            //rút gọn link thanh toán giỏ hàng
            routes.MapRoute(
               name: "checkout",
               url: "checkout",
               defaults: new { controller = "Cart", action = "Checkout" }
            );
            //rút gọn link tin tức
            routes.MapRoute(
              name: "news all",
              url: "all-post",
              defaults: new { controller = "News", action = "AllListNews" } 
           );
            //rút gọn link search bài viêt
            routes.MapRoute(
              name: "search post",
              url: "search-post",
              defaults: new { controller = "News", action = "SearchResult" }
           );
            //rút gọn link tin tức
            routes.MapRoute(
              name: "news",
              url: "news",
              defaults: new { controller = "News", action = "NewsIndex" }
           );
            //tac giả
            //rút gọn link tin tức
            routes.MapRoute(
              name: "news author",
              url: "news/author/{Name}",
              defaults: new { controller = "News", action = "ListNews" }
           );
            //rút gọn link chi tiết tin tức
            routes.MapRoute(
              name: "News detail",
              url: "post/{slug}",
              defaults: new { controller = "News", action = "NewsDetail" }
           );
            //rút gọn link chi tiết tin tức
            routes.MapRoute(
              name: "list news tag",
              url: "post/tags/{slug}",
              defaults: new { controller = "News", action = "NewsDetail" }
           );
           routes.MapRoute(
              name: "list tags",
              url: "tags/{slug}",
              defaults: new { controller = "News", action = "ListNewsTags" }
           );
            //danh sách sản phẩm mới của tin tức
            routes.MapRoute(
               name: "list product news",
               url: "news/product_list/",
               defaults: new { controller = "News", action = "PostProductRecent" }
            );
            
            //bài viết của sản phẩm
            routes.MapRoute(
              name: "list post prodyct",
              url: "news/product/{slug}",
              defaults: new { controller = "News", action = "PostProduct" }
           );
            //list bài viết của author
            routes.MapRoute(
              name: "author post",
              url: "news/author/",
              defaults: new { controller = "News", action = "Author" }
           );
            //rút gọn link danh dánh bài viết của loại tin
            routes.MapRoute(
              name: "list news category",
              url: "news/list_post/{slug}",
              defaults: new { controller = "News", action = "ListNews"} 
           );
            //rút gọn link loại tin của danh mục
            routes.MapRoute(
              name: "news category",
              url: "news/genre/{slug}",
              defaults: new { controller = "News", action = "ListNewsCategory"} 
           );
            //rút gọn link khuyến mãi
            routes.MapRoute(
              name: "promotion",
              url: "khuyen-mai",
              defaults: new { controller = "Campaign", action = "Listbanner" }
           );
            //rút gọn link chi tiet sản phẩm khuyến mãi
            routes.MapRoute(
              name: "promotion detail",
              url: "promo/{slug}",
              defaults: new { controller = "Campaign", action = "Bannerdetail" }
           );
//------------------------- start rút gọn đăng nhập, đăng ký, thông tin cá nhân,...------------------
            //rút gọn link đăng nhập
            routes.MapRoute(
             name: "signin",
              url: "account/singin",
              defaults: new { controller = "Account", action = "SignIn" }
           );
            //lịch sử bình luận
            routes.MapRoute(
             name: "comment history",
              url: "account/comment-history",
              defaults: new { controller = "Account", action = "CommentHistory" }
           );
            //lịch sử phản hồi
            routes.MapRoute(
             name: "reply history",
              url: "account/reply-history",
              defaults: new { controller = "Account", action = "ReplyHistory" }
           );
            //lịch sử đánh giá sản phẩm
            routes.MapRoute(
             name: "feedbacks history",
              url: "account/feedbacks-history",
              defaults: new { controller = "Account", action = "FeedbackHistory" }
           );
            //rút gọn link đăng ký
            routes.MapRoute(
                name: "registration",
                url: "account/register",
                defaults: new { controller = "Account", action = "Register" }
            );
            //
            routes.MapRoute(
                name: "verification account",
                url: "account/email_verification_required",
                defaults: new { controller = "Account", action = "EmailverificationRequired" }
            );
            //rút gọn link quên mật khẩu
            routes.MapRoute(
              name: "forgotpassword",
              url: "account/recover_password",
              defaults: new { controller = "Account", action = "ForgotPassword" }
           );
            //thay đổi mật khẩu
            routes.MapRoute(
              name: "changepassword",
              url: "account/change_password",
              defaults: new { controller = "Account", action = "ChangePassword" }
           );

            //rút gọn link thông tin cá nhân
            routes.MapRoute(
              name: "profile",
              url: "account/profile",
              defaults: new { controller = "Account", action = "Editprofile" }
           );

            //rút gọn link quản lý đơn hàng
            routes.MapRoute(
              name: "tracking orders",
              url: "account/order-list",
              defaults: new { controller = "Account", action = "TrackingOrder" }
           );
            
          //rút gọn link quản lý địa chỉ
            routes.MapRoute(
              name: "address mannager",
              url: "account/address-list",
              defaults: new { controller = "Account", action = "AddressManager" }
           );
            //rút gọn link quản lý địa chỉ
            routes.MapRoute(
              name: "order detail",
              url: "account/order_detail/{id}",
              defaults: new { controller = "Account", action = "TrackingOrderDetail", id = UrlParameter.Optional }
           );

            //cập nhật mật khẩu mới
            routes.MapRoute(
              name: "Reset password",
              url: "account/recover_password_update",
              defaults: new { controller = "Account", action = "ResetPassword" }
           );
//------------------------- end rút gọn đăng nhập, đăng ký, thông tin cá nhân,...------------------
            //gửi yêu cầu hồ trợ
            routes.MapRoute(
              name: "sent request",
              url: "ho-tro",
              defaults: new { controller = "Home", action = "SentRequest" }
           );
            //set error 404
            routes.MapRoute(
              name: "Page Not Found",
              url: "pagenotfound",
              defaults: new { controller = "Home", action = "PageNotFound" }
           );
            //link mặc định khi khởi động
            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
