var delmodal = $('#deleteModal');
var idde;
var deleteConfirm = function (id, email,countorder,countnews,newscomment,countfeedback) {
    delmodal.find('.lable_email').text(email);
    delmodal.find('.count_order').text(countorder);
    delmodal.find('.countnews').text(countnews);
    delmodal.find('.countnewscomment').text(newscomment);
    delmodal.find('.countfeedback').text(countfeedback);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Roles/RolesDelete',
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
                    title: '!Tài khoản đã có đơn hàng không thể xoá'
                })
            } 
            else if (result == "ExistComment"){
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
                    title: '!Tài khoản đã bình luân bài viết không thể xoá'
                })
            }
            else if (result == "ExistPost") {
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
                    title: '!Còn bài biết của tác giả'
                })
            }
            else if (result == "ExistFeedback"){
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
                    title: '!Tài khoản đã đánh giá sản phẩm không thể xoá'
                })
            }
            else {
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
                title: result.Message
            })
            setTimeout(function () {
                window.location.reload();
            }, 1000);
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
