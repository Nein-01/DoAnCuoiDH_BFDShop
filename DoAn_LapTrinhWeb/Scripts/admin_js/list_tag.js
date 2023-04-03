var newModal = $('#addModal');
var delmodal = $('#deleteModal');
var editModal = $('#editModal');

$('#addTag').click(function () {
    $('#TagName').val(null);
    newModal.modal('show');
});
$('#ConfirmAdd').click(function () {
    var sname = $('#TagName').val();
    if (sname == "") {
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
            title: 'Nhập đầy đủ thông tin'
        })
    }
    else {
        $.ajax({
            type: "post",
            url: '/TagsAdmin/TagCreate',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ name: sname}),
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
                        title: 'Tag bị trùng tên hoặc bạn không có quyền thêm Tag'
                    })
                }
                else {
                    newModal.modal('hide');
                    setTimeout(function () {
                        window.location.reload();
                    }, 50);
                }
            },
            error: function (recData) {
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
        });
    }

});
var ide;
var EditTag = function (id, name) {
    ide = id;
    $('#TagNameEdit').val(name);
    editModal.modal('show');
}
$('#confirmeditBtn').click(function () {
    var sname2 = $('#TagNameEdit').val(); var sname2 = $('#TagNameEdit').val();
    if (sname2 == "") {
        alert("Hãy nhập đủ thông tin");
    }
    else {
        $.ajax({
            type: "post",
            url: '/TagsAdmin/TagEdit',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: ide, name: sname2 }),
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
                        icon: 'warning',
                        title: '!Tag đã tồn tại hoặc bạn không có quyền sửa'
                    })
                }
                else {
                    editModal.modal('hide');
                    setTimeout(function () {
                        window.location.reload();
                    }, 50);
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
                    title: 'Sửa thất bại'
                })
            }
        });
    }

});
////
var idde;
var deleteConfirm = function (id, title,countpost) {
    delmodal.find('.lable_tag').text(title);
    delmodal.find('.countpost_tag').text(countpost);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/TagsAdmin/DeleteTag',
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
                    title: 'Tag không thể xóa'
                })
            }
            else {
                setTimeout(function () {
                    window.location.reload();
                }, 50);
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
                title: 'Xóa thất bại'
            })
        }
    });
});
