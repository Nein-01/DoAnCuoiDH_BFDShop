var clicked = 0;

$(".toggle-password").click(function (e) {
    e.preventDefault();

    $(this).toggleClass("toggle-password");
    if (clicked == 0) {
        $(this).html('<span class="material-icons">visibility_off</span >');
        clicked = 1;
    } else {
        $(this).html('<span class="material-icons">visibility</span >');
        clicked = 0;
    }

    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});