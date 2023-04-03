var fixmeTop = $('.fixme').offset().top;       // get initial position of the element
$(window).scroll(function() {                  // assign scroll event listener
    var currentScroll = $(window).scrollTop(); // get current position
    if (currentScroll > fixmeTop) {
        // apply position: fixed if you
        $('.fixme').css({
            // scroll to that element or below it
            position: 'fixed',
            top: '-2px',
            right:'0',
            left: '0',
        });
    } else {
        // apply position: static
        $('.fixme').css({
            // if you scroll above it
            position: 'sticky',
        });
    }
});
//fixed header đã tắt để bật lại bạn vui lòng vô shared/header_layout.cshml vả bỏ comment class @*fixme*@ tại headerbottom