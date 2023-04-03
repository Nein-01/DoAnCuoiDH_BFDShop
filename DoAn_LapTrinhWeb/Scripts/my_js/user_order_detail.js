$('#open_cancle_order').click(function(){
    $('#cancle_my_order').modal('show')
})
$('#btn__back').click(function () {
    $('#cancle_my_order').modal('hide')
})
$("#btn_cancle__order").click(function () {
    var data = $("#formcancle_order").serialize();
    $("#cancle_my_order").modal("hide")
    $.ajax({
        type: "post",
        url: "/Account/CancleOrder",
        data: data,
        success: function (result) {
            if (result == true) {
                $('.order_cancled').text('Đơn hàng đã bị hủy');
                $('.order_cancled').addClass('alert-danger');
                $('.order_cancled').removeClass('alert-warning');
                $(".remove_order_cancled").remove();
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
                    icon: 'success',
                    title: 'Huỷ đơn hàng thành công'
                })
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
                    title: 'Lỗi, đơn hàng không thể hủy'
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
                title: 'Gửi thất bại'
            })
        }
    })
})
