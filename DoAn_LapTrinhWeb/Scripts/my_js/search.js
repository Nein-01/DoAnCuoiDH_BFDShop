$(document).ready(function () {
    //search jquery auto complete
    $("#product_name").autocomplete({
        delay: 500,
        source: function (request, response) {
            $.ajax({
                url: "/Products/GetProductSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.product_name, value: item.product_name, slug: item.slug, Image: item.Image, price: item.price };
                    }));
                    if (data.length == 0) {
                        $('.spinner').show();
                        const Toast = Swal.mixin({
                            toast: true,
                            position: 'center',
                            showConfirmButton: false,
                            timer: 1500,
                            didOpen: (toast) => {
                                toast.addEventListener('mouseenter', Swal.stopTimer)
                                toast.addEventListener('mouseleave', Swal.resumeTimer)
                            }
                        })
                        Toast.fire({
                            icon: 'error',
                            title: 'Không có kết quả tìm kiếm'
                        })
                    }
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.Image + " alt='img' class='img_search' />")
                    .append('<span class="lable_search">' + item.label + '</span>')
                    .append('<span class="color-price format_Price2" id="product_price">' + addPeriod(item.price)+'₫'+ '</span>')
                    .appendTo(ul);             
            };
        },
        select: function (event, ui) {
            window.location.href = '/product/' + ui.item.slug;
            return false;
        }
    })
})
//http://www.mredkj.com/javascript/nfbasic.html
function addPeriod(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return x1 + x2;
}
