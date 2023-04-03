var delmodal = $('#deleteModal');
var idde;
var deleteConfirm = function (id, title,countbanner,countorder,countfeedback) {
    delmodal.find('.lable_product').text(title);
    delmodal.find('.count_banner').text(countbanner);
    delmodal.find('.countorder_product').text(countorder);
    delmodal.find('.countfeedback_product').text(countfeedback);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/ProductsAdmin/ProductDelete',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (result) {
            if (result == "ExitOrder") {
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
                    title: '!Còn đơn hàng chứa sản phẩm này'
                })
            } 
            else if (result == "ExitBanner"){
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
                    title: '!Sản phẩm đang trong CTKM không thể xoá'
                })
            }
            else if (result == "ExitFeedback"){
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
                    title: '!Sản phẩm có đánh giá không thể xoá'
                })
            }
            else if (result == "false") {
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
                    title: 'Bạn không có quyền xóa sản phẩm'
                })
            }
            else {
                setTimeout(function () {
                    window.location.reload();
                }, 1);
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
