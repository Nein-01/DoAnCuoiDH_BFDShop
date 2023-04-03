using System.Web.Optimization;

namespace DoAn_LapTrinhWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //--------------------------------------------------------JS---------------------------------------------------------------
            //fixed head der,...
            bundles.Add(new ScriptBundle("~/bundles/fixedheader").Include("~/Scripts/my_js/fixedheader.js"));
            //button share link,fb,messenger,...
            bundles.Add(new ScriptBundle("~/bundles/buttonsharelink").Include("~/Scripts/my_js/button_share_fb.js"));
            //chat plugin fb messenger cho website
            bundles.Add(new ScriptBundle("~/bundles/chatpluginfb").Include("~/Scripts/my_js/chat_plugin_fb.js"));
            //
            bundles.Add(new ScriptBundle("~/bundles/jquerymin").Include("~/Scripts/jquery-3.6.0.min.js"));
            //
            bundles.Add(new ScriptBundle("~/bundles/bootstrapmin").Include("~/Scripts/my_js/bootstrap.min.js"));
            //validation form
            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidation").Include(
            "~/Scripts/jquery.validate.min.js","~/Scripts/jquery.validate.unobtrusive.min.js"));
            //
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/my_js/jquery-ui-1.12.1.js"));
            //hiện thông báo có thể sử dụng 1 trong 3::toastr - sweet alert 2 - jquery toast
            bundles.Add(new ScriptBundle("~/bundles/toast").Include("~/Scripts/my_js/sweetalert2.min.js",
            "~/Scripts/my_js/jquery.toast.js","~/Scripts/my_js/toastr.min.js", "~/Scripts/my_js/bootbox.js"));
            //ràng buộc form nhập
            bundles.Add(new ScriptBundle("~/bundles/jqueryinputmask").Include("~/Scripts/my_js/jquery.inputmask.js","~/Scripts/my_js/inputmask.js"));
            //quay trở lại top khi bấm vào mũi tên góc dưới phải màn hình
            bundles.Add(new ScriptBundle("~/bundles/backtotop").Include("~/Scripts/my_js/back_to_top.js"));
            //hiển thị tag giảm giá. ở từng sản phẩm có chương trình giảm giá
            bundles.Add(new ScriptBundle("~/bundles/discountstag").Include("~/Scripts/my_js/discount.js"));
            //xem ảnh full màn, dùng trong product detail khi bấm vào từng ảnh sẽ hiển thị ảnh fullscreen
            bundles.Add(new ScriptBundle("~/bundles/fslightbox").Include("~/Scripts/fslightbox.js"));
            //hiển thị ngày tháng và có thể tùy chỉnh cách hiển thị: chỉ hiển thị ngày thắng hoặc giờ
            bundles.Add(new ScriptBundle("~/bundles/jquerydatetimepicker").Include("~/Scripts/my_js/jquery.datetimepicker.js"));
            //
            bundles.Add(new ScriptBundle("~/bundles/poppermin").Include("~/Scripts/my_js/popper.min.js"));
            //
            bundles.Add(new ScriptBundle("~/bundles/cookie").Include("~/Scripts/my_js/cookie.js"));
            //dùng liên quan đến add sản phẩm vào giỏ hàng
            bundles.Add(new ScriptBundle("~/bundles/common").Include("~/Scripts/my_js/common.js"));
            //hiển thị hình ảnh dưới dạng slide
            bundles.Add(new ScriptBundle("~/bundles/carouselmin").Include("~/Scripts/my_js/owl.carousel.min.js"));
            //custom thông báo
            bundles.Add(new ScriptBundle("~/bundles/customtoastr").Include("~/Scripts/my_js/custom_toastr.js"));
            //custom validation form: ràng buộc nhập  email, số điện thoại sử dùng kèm jqueryinputmask.js và inputmask.js
            bundles.Add(new ScriptBundle("~/bundles/custominputform").Include("~/Scripts/my_js/custom_input_form.js"));
            //custom search: gợi ý khi gõ chữ vào thanh searchcustom search: gợi ý khi gõ chữ vào thanh search
            bundles.Add(new ScriptBundle("~/bundles/customsearch").Include("~/Scripts/my_js/search.js"));
            //check ràng buộc đăng nhập, đăng ký,...
            bundles.Add(new ScriptBundle("~/bundles/checkvalidaccount").Include("~/Scripts/my_js/checkuseraccount.js"));
            //Popup first load page
            bundles.Add(new ScriptBundle("~/bundles/firstloadpage").Include("~/Scripts/my_js/jquery.firstVisitPopup.js", "~/Scripts/my_js/popup_first_loadpage.js"));
            //------------------------------------------CSS--------------------------------------
            //Style css
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/my_css/bootstrap.min.css", "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/jquery-ui").Include("~/Content/my_css/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/bundles/responsiveweb").Include("~/Content/my_css/responsive.css"));

            bundles.Add(new StyleBundle("~/bundles/colorsweb").Include("~/Content/my_css/colors1.css"));

            bundles.Add(new StyleBundle("~/bundles/toasts").Include("~/Content/my_css/toastr.min.css", "~/Content/my_css/jquery.toast.css"));

            bundles.Add(new StyleBundle("~/bundles/jquerydatetimepicker").Include("~/Content/my_css/jquery.datetimepicker.css"));

            bundles.Add(new StyleBundle("~/bundles/font-awesome").Include("~/Content/my_css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/bundles/owl").Include("~/Content/my_css/owl.carousel.min.css", "~/Content/my_css/owl.theme.default.css"));

        }
    }
}