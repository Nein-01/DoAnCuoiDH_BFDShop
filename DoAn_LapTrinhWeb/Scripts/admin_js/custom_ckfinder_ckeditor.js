CKEDITOR.replace("ckedittor_uploadimage", {
    customConfig: "/Scripts/admin_js/ckeditor/config.js",
});


$("#selectimage").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkimages").val(fileurl);
    };
    finder.popup();
});



  
