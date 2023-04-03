$("#selectimage1").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkimages1").val(fileurl);
    };
    finder.popup();
});