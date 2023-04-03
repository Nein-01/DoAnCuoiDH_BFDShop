$('#Submitbtn').click(function () {
    var data = $("#form_add_role").serialize();
    $.ajax({
        type: "POST",
        url: '/Roles/CreateRole',
        data: data,
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
                    title: 'Bạn không có quyền sử dụng chức năng này'
                })
            }
            else {
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
var editmodal = $("#editformmodal");
var editrole = function (role_id, role)
{
    $("#role_id").val(role_id);
    $("#role_name").val(role);
    editmodal.modal("show");
}
$("editformmodal").on('hidden', function () {
    $("#form_edit_role")[0].reset();
});

$('#SubmitEditbtn').click(function () {
    var data = $("#form_edit_role").serialize();
    $.ajax({
        type: "POST",
        url: '/Roles/RoleEdit',
        data: data,
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
                    title: 'Bạn không có quyền sử dụng chức năng này'
                })
            }
            else {
                editmodal.modal("hide");
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

