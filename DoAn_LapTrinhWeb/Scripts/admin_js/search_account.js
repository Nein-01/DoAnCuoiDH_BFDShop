$(document).ready(function () {
    $("#Email").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Roles/GeUserSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Email, value: item.Email };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "Không tìm thấy tài khoản",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#Email").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#Email").val(ui.item.label);
            return false;
        }
    })

})