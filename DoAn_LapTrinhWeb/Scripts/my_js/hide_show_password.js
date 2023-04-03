//hiện ẩn password: đăng ký, đăng nhập,...
$(function () {
    $('.hide-show').show();
    $('.hide-show span').addClass('show')

    $('.hide-show span').click(function () {
        if ($(this).hasClass('show')) {
            $(this).text('Hide');
            $('input[name="login[password]"]').attr('type', 'text');
            $(this).removeClass('show');
        } else {
            $(this).text('Show');
            $('input[name="login[password]"]').attr('type', 'password');
            $(this).addClass('show');
        }
    });

    $('form button[type="submit"]').on('click', function () {
        $('.hide-show span').text('Show').addClass('show');
        $('.hide-show').parent().find('input[name="login[password]"]').attr('type', 'password');
    });
});
