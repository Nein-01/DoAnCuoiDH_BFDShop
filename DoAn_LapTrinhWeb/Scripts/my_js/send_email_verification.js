var sendmail = function () {
    var ac_idd = $("#acc_id").val();
    var data = $("#formresend").serialize();
    if (ac_idd != "") {
        $.ajax({
            type: "post",
            url: "/Account/ReSendVerification",
            data: data,
            success: function () {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 3000,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'success',
                    title: 'Đã gửi code xác thực đến Email của bạn'
                })
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
    }
}
