var amodal = $('#addModal');
var delmodal = $('#deleteModal');
var idde;
var idfix;
var deleteConfirm = function (id, title) {
    delmodal.find('.title_stickypost').text(title);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/NewsAdmin/DeleteStickyPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (recData) {
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
                title: recData.Message
            })
            if (recData.reload != false) {
                setTimeout(function () {
                    window.location.reload();
                }, 1500);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/>Không xóa được<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
});
$('#addHot').click(function () {
    $('#IDPost').val(null);
    amodal.modal('show');

});
$('#addBtn').click(function () {
    var _sticky_start = $('#dt_disount_start').val();
    var _sticky_end = $('#dt_disount_end').val();
    var post_id = $('#post_id').val();
    amodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/NewsAdmin/CreateStickyPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: post_id, sticky_start: _sticky_start, sticky_end: _sticky_end}),
        dataType: "json",
        success: function (recData) {
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
                title: recData.Message
            })
            if (recData.reload != false) {
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
                icon: 'warning',
                title: '!Lỗi, thêm thất bại'
            })
        }
    });
});

