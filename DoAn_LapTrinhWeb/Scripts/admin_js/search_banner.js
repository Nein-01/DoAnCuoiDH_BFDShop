$(document).ready(function () {
    $("#search_banner").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Banners/GetBannerSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { name: item.banner_name, image: item.image_thumbnail };
                    }))
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.image + " alt='img' class='img_search' />")
                    .append('<span class="lable_search">' + item.name + '</span>')
                    .appendTo(ul);
            };
        },
        minLength: 0,
        messages: {
            noResults: "Không tìm thấy sản phẩm",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        },
    })
})