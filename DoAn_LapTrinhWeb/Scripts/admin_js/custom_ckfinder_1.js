$("#selectimage2").on('click', function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileurl) {
        $("#linkimages2").val(fileurl);
    };
    finder.popup();
});