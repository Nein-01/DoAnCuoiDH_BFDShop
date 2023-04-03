$(document).ready(function () {
    $("#banner_detail").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/BannerDetails/GetBannerDetailSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.product_name, value: item.product_name, slug: item.slug, image: item.image };
                    }))

                }

            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.image + " alt='img' class='img_search' />")
                    .append('<span class="lable_search">' + item.label + '</span>')
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