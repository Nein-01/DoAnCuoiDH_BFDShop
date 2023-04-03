setTimeout(function () {
    $('div')
        .removeClass('loading')
        .addClass('loaded');
}, 1000);//set time tùy theo sở thích của bạn. ở đây mình để 200ms = 0.2s
//mục đích loading page là để đõ bị giật layout khi chuyển page, nguyên do bị giật do page chưa lấp đầy nội dung