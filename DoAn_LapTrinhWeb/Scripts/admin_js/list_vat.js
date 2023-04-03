var newModal = $('#addModal');
var delmodal = $('#deleteModal');
var editModal = $('#editModal');

$('#addVAT').click(function () {
    $('#VATName').val(null);
    newModal.modal('show');
});
$('#ConfirmAdd').click(function () {
    var sname = $('#TaxesName').val();
    var svalue = $('#TaxesValue').val();
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
            title: 'Nhập tên VAT'
        })
    }
    else if (svalue == "") {
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
            title: 'Nhập phần trăm VAT'
        })
    }
    else {
        $.ajax({
            type: "post",
            url: '/Taxes/TaxesCreate',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ name: sname, value: svalue}),
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    newModal.modal('hide');
                    setTimeout(function () {
                        window.location.reload();
                    }, 100);
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
                        title: 'Bạn không có quyền thêm mới'
                    })
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
                    title: 'Thêm thất bại'
                })
            }
        });
    }

});
var ide;
var EditTag = function (id, name,value) {
    ide = id;
    $('#TaxesNameEdit').val(name);
    $('#TaxesValueEdit').val(value);
    editModal.modal('show');
}
$('#confirmeditBtn').click(function () {
    var sname2 = $('#TaxesNameEdit').val();
    var svalue2 = $('#TaxesValueEdit').val();
    if (sname2 == "") {
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
            title: 'Nhập tên VAT'
        })
    }
    else if (svalue2 == "") {
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
            title: 'Nhập phần trăm VAT'
        })
    }
    else {
        $.ajax({
            type: "post",
            url: '/Taxes/TaxesEdit',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: ide, name: sname2, value: svalue2}),
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    editModal.modal('hide');
                    setTimeout(function () {
                        window.location.reload();
                    }, 100);
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
                        title: 'Bạn không có quyền chỉnh sửa'
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
    delmodal.find('.lable_vat').text(title);
    delmodal.find('.count_product_vat').text(countpost);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Taxes/TaxesDelete',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (result) {
            if (result == false) {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2800,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Không thể xóa'
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
                title: '!Không xoá được'
            })
        }
    });
});
