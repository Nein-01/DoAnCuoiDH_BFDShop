$(document).ready(function () {
    $("#product_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Product_Image/GetProductImageSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { produtct_id: item.product_id, product_name: item.product_name, image_id: item.product_image_id };
                    }))
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append('<span class="lable_search">' + item.image_id + '</span>')
                    .append('<span class="color-price" >' + item.produtct_id + '</span>')
                    .append('<span class="color-price" >' + item.product_name + '</span>')
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