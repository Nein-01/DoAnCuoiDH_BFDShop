$('#sendcode').click(function () {
    $.ajax({
        type: "post",
        url: '/Discounts/SendMailBirthDayCode',
        dataType: "json",
        success: function (result) {
            if (result == true) {
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
                    title: 'Đã gửi code sinh nhật thành công'
                })
                setTimeout(function () {
                    window.location.reload();
                }, 1800);
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
                    title: 'Không còn user nào có sinh nhật trong tháng để nhận code'
                })
                setTimeout(function () {
                    window.location.reload();
                }, 1500);
            }
        },
        error: function (result) {
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
            setTimeout(function () {
                window.location.reload();
            }, 1500);
        }
    });
});
