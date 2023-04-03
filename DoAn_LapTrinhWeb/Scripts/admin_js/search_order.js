$(document).ready(function () {
    $("#order_id").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Orders/GetOrderSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.order_id, value: item.order_id };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "Không tìm thấy đơn hàng",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#order_id").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#order_id").val(ui.item.label);
            return false;
        }
    })

})