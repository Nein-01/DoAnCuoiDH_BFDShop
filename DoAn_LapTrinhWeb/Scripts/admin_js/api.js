var editModal = $('#editModal');
var ide;
var EditAPI = function (id, apiname,clientid,clientsecret,description) {
    ide = id;
    $('#apiname').val(apiname);
    $('#clientid').val(clientid);
    $('#clientscret').val(clientsecret);
    $('#decription_api').html(description);
    editModal.modal('show');
}
$('#confirmeditBtn').click(function () {
    var _apiname = $('#apiname').val();
    var _clientid = $('#clientid').val();
    var _clientsceret = $('#clientscret').val();
    if (_apiname == "") {
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
            title: 'Nhập tên API'
        })
    }
    else {
        $.ajax({
            type: "post",
            url: '/API/APIEdit',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: ide, apiname: _apiname, client_id: _clientid, client_secret: _clientsceret }),
            dataType: "json",
            success: function (result) {
                editModal.modal('hide');
                setTimeout(function () {
                    window.location.reload();
                }, 50);
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
                    title: 'Nhập thông tin còn thiếu'
                })
            }
        });
    }
});
var changeStt = function (xthis) {
    var xid = xthis.id;
    var st = xthis.checked;
    $.ajax({
        type: "POST",
        url: '/API/ChangeStatus',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: xid, state: st }),
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
                    title: 'Thay đổi trạng thái thành công'
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
                    icon: 'error',
                    title: 'Bạn không có quyền đổi trạng thái'
                })
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 1500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Thay đổi trạng thái không thành công'
            })
        }
    });
}