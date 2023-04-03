$(document).ready(function () {
    $("#news_title").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/News/GetPostSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.news_title, value: item.news_title, slug: item.news_slug, image: item.image};
                    }))
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
                    .append("<img src=" + item.image + " alt='img' class='img_search'/>")
                    .append('<span class="lable_searchpost">' + item.label + '</span>')
                    .appendTo(ul);
            };
        },
        select: function (event, ui) {
            window.location.href = '/post/' + ui.item.slug;
            return false;
        }
    })

})