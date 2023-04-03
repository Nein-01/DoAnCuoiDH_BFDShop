var newModal = $('#addModal');
var delmodal = $('#deleteModalBanner');

$('#create_banner_detail').click(function () {
    $('#TagName').val(null);
    newModal.modal('show');
});
var create_banner_product = function () {
    var banner = $('#banner_select').val();
    var product = $('#product_select').val();
    if (banner == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2500,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Chọn chương trình khuyến mãi'
        })
    } else if (product == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2500,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Chọn sản phẩm cần thêm'
        })
    }
    else {
        var data = $('#product_banner_form').serialize();
        $.ajax({
            type: "get",
            url: '/BannerDetails/CreateProductBanner',
            data: data,
            success: function (result) {
                newModal.modal('hide');
                if (result == true) {
                    setTimeout(function () {
                        window.location.reload();
                    }, 100);
                }
                else {
                    newModal.modal('hide');
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top',
                        showConfirmButton: false,
                        timer: 2500,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })
                    Toast.fire({
                        icon: 'warning',
                        title: 'Lỗi'
                    })
                }
            },
            error: function () {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'warning',
                    title: 'Thêm thất bại'
                })
            }
        });
    }

};
////
var idde;
var deleteConfirm = function (id, bannertitle,productname) {
    delmodal.find('.lable_banner').text(bannertitle);
    delmodal.find('.productnametext').text(productname);
    delmodal.modal('show');
    idde = id;
}
$('#deleteProductBannerBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/BannerDetails/DeleteProductBanner',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (result) {
            if (result == true) {

                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1000,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'success',
                    title: 'Xoá thành công'
                })
                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Xoá thất bại'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'error',
                title: 'Lỗi'
            })
        }
    });
});
