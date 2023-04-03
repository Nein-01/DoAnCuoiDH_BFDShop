var delmodal = $('#CancleModal');
var CancleConfirm = function (id, name, totalprice) {
    delmodal.find('.lable_order').text(id);
    delmodal.find('.customername').text(name);
    delmodal.find('.total_price').text(totalprice);
    delmodal.modal('show');
    idde = id;
}
var reportmodal = $('#ReportModal');
$('#OpenReport').click(function () {
    reportmodal.modal('show')
});
$('.CloseReport').click(function () {
    reportmodal.modal('hide')
});
$('#CancleBtn').click(function () {
    $.ajax({
        type: "POST",
        url: '/Orders/CancleOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (result) {
            if (result == false) {
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
                    title: '!Đơn hàng đã hoàn thành không thể huỷ'
                })
            }
            else {
                delmodal.modal('hide');
                setTimeout(function () {
                    window.location.reload();
                }, 100);
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
                title: '!Lỗi'
            })
        }
    });
});
