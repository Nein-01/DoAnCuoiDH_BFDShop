$(document).ready(function () {
    $(".btn-inc").click(function () {
        setTimeout(function () {
            $('body').addClass('loaded');
        }, 300);

        $('#loader-wrapper').removeAttr("hidden");
        $('#loader').removeAttr("hidden");
        $('.loader-section').removeAttr("hidden");

        $('body').removeClass('loaded');
        $('#loader-wrapper').attr("hidden");
        $('#loader').attr("hidden");
        $('.loader-section').attr("hidden");
    });

    $(".btn-dec").click(function () {
        setTimeout(function () {
            $('body').addClass('loaded');
        }, 300);
        $('#loader-wrapper').removeAttr("hidden");
        $('#loader').removeAttr("hidden");
        $('.loader-section').removeAttr("hidden");

        $('body').removeClass('loaded');
        $('#loader-wrapper').attr("hidden");
        $('#loader').attr("hidden");
        $('.loader-section').attr("hidden");
    });



    $("#submit_order").click(function () {
        $('body').removeClass('loaded');
        $('#loader-wrapper').removeAttr("hidden");
        $('#loader').removeAttr("hidden");
    });


    $(".btnloader").click(function () {
        $('.loader').addClass('is-active');
    });
});
