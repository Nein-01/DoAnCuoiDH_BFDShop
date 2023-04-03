CKEDITOR.replace("ckedittor_uploadimage1", {
    customConfig: "/Scripts/admin_js/ckeditor/config.js"
});
$("#selectimage1").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkimages1").val(fileurl);
    };
    finder.popup();
});