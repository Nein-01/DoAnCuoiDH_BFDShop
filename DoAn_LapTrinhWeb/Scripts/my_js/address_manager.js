$(document).ready(function () {
    $('#province').change(function () {
        $('#district').removeClass('cursor-disable');
        $('#district').removeAttr('disabled');
        $.get("/Account/GetDistrictsList", { province_id: $('#province').val() }, function (data) {
            $('#district').html("<option value>Quận/Huyện</option>");
            $.each(data, function (index, row) {
                $('#district').append("<option value='" + row.district_id + "'>" + row.type + " " + row.district_name + "</option>")
            });
        });
    })
    $('#district').change(function () {
        $('#ward').removeClass('cursor-disable');
        $('#ward').removeAttr('disabled');
        $.get("/Account/GetWardsList", { district_id: $('#district').val() }, function (data) {
            $('#ward').html("<option value>Phường/Xã</option>");
            $.each(data, function (index, row) {
                $('#ward').append("<option value='" + row.ward_id + "'>" + row.type + " " + row.ward_name + "</option>")
            });
        });
    })
});

var createmodal = $('#ModalCreate');
$('#popupcreateaddress').click(function () {
    createmodal.modal('show');
})
$('#closemodal').click(function () {
    editModal.modal('hide');
});
$('#closemodal1').click(function () {
    editModal.modal('hide');
});
$('#closemodal4').click(function () {
    editModal2.modal('hide');
});
$('#closemodal5').click(function () {
    editModal2.modal('hide');
});

$('#closemodal3').click(function () {
    createmodal.modal('hide');
});

var SaveAddress = function () {
    var username = $("#address_name").val();
    var phonenumber = $("#address_phone").val();
    var province = $("#province").val();
    var disctric = $("#district").val();
    var address = $("#address_content").val();
    var ward = $("#ward").val();
    if (username == "" || phonenumber == "" || province == "" || disctric == "" || ward == "" || address == "") {
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
            title: 'Nhập thông tin còn thiếu'
        })
        return false;
    }
    else if (phonenumber.length < 10 || phonenumber.length > 10) {
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
            title: 'Số điện thoại phải đúng 10 chữ số'
        })
        return false;
    }
    else if (username.length > 20) {
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
            title: 'Họ tên không quá 20 ký tự'
        })
        return false;
    }
    else if (address.length > 50) {
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
            title: 'Địa chỉ cụ thể không quá 50 ký tự'
        })
        return false;
    }
    else {
    var data = $("#create_address").serialize();
    $.ajax({
        type: "POST",
        url: "/Account/AddressCreate", //kiểm tra tồn tại username, username và password đã trùng chưa (kiểm tra ở acition checksignin của cotroller Account)
        data: data,
        success: function (result) {
            createmodal.modal('hide');
            if (result == true) {
                setTimeout(function () {
                    window.location.reload();
                }, 1);
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
                    title: 'Lỗi'
                })
            }
        }
    })
    }
}
//sửa thông tin địa chỉ
$(document).ready(function () {
    $('#province_edit').change(function () {
        $('#district_edit').removeClass('cursor-disable');
        $('#district_edit').removeAttr('disabled');
        $.get("/Account/GetDistrictsList", { province_id: $('#province_edit').val() }, function (data) {
            $('#district_edit').html("<option value>Quận/Huyện</option>");
            $.each(data, function (index, row) {
                $('#district_edit').append("<option value='" + row.district_id + "'>" + row.type + " " + row.district_name + "</option>")
            });
        });
    })
    $('#district_edit').change(function () {
        $('#ward_edit').removeClass('cursor-disable');
        $('#ward_edit').removeAttr('disabled');
        $.get("/Account/GetWardsList", { district_id: $('#district_edit').val() }, function (data) {
            $('#ward_edit').html("<option value>Phường/ Xã</option>");
            $.each(data, function (index, row) {
                $('#ward_edit').append("<option value='" + row.ward_id + "'>" + row.type + " " + row.ward_name + "</option>")
            });
        });
    })
});
var ide;
var editModal = $('#EditAddress');
var EditAddress = function (id, username, phonenumber, province_id, district_id, ward_id, address_content) {
    ide = id;
    $('#province_edit').val(province_id);
    $('#address_name_edit').val(username);
    $('#district_edit').val(district_id);
    $('#ward_edit').val(ward_id);
    $('#address_content_edit').val(address_content);
    $('#address_phone_edit').val(phonenumber);
    editModal.modal('show');
}
$('#confirmeditBtn').click(function () {
    var _province_id = $('#province_edit').val();
    var _username = $('#address_name_edit').val();
    var _district_id = $('#district_edit').val();
    var _ward_id = $('#ward_edit').val();
    var _address_content = $('#address_content_edit').val();
    var _phonenumber = $('#address_phone_edit').val();
    if (_province_id == "" || _username == "" || _district_id == "" || _ward_id == "" || _address_content == "" || _phonenumber == "") {
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
            title: 'Hãy nhập đầy đủ thông tin'
        })
    }
    else if (_address_content.length > 50) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2000,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Địa chỉ cụ thể không quá 50 ký tự'
        })
        $('#confirmeditBtn').attr('disabled');
    }
    else if (_phonenumber.length < 10 || _phonenumber.length>10) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2000,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Số điện thoại phải đúng 10 chữ số'
        })
    }
    else if (_username.length > 20) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2000,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Họ tên không quá 20 ký tự'
        })
    }
    else {
        $.ajax({
            type: "post",
            url: '/Account/AddressEdit',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: ide, username: _username, province_id: _province_id, district_id: _district_id, ward_id: _ward_id, address_content: _address_content, phonenumber: _phonenumber}),
            dataType: "json",
            success: function (result) {
                if (result == true) {
                    editModal.modal('hide');
                    setTimeout(function () {
                        window.location.reload();
                    }, 1);
                }
                else {
                    editModal.modal('hide');
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
                        title: 'Lối! không tìm thấy ID'
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

//xóa địa chỉ
var delmodal = $('#deleteModal');
var idde;
var deleteConfirm = function (id) {
    delmodal.modal('show');
    idde = id;
}
$("#cancle_delete_address").click(function () {
    delmodal.modal('hide');
});
$('#btndelete_address').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/Account/AddressDelete',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                setTimeout(function () {
                    window.location.reload();
                }, 1);
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
                    title: '!Lỗi'
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
                icon: 'error',
                title: '!Lỗi'
            })
        }
    });
});
//cập nhật lại thông tin địa chỉ đơn hàng
var editModal2 = $('#EditOrderAddress');
var Edit = function (id, username, phonenumber,province,district,ward, address_content) {
    $('#name_order_address').val(username);
    $('#phone_order_address').val(phonenumber);
    $('#province_order_address').val(province);
    $('#district_order_address').val(district);
    $('#ward_order_address').val(ward);
    $('#order_address_content').val(address_content);
    editModal2.modal('show');
    ide = id 
    $('#btnaddressedit').click(function () {
        var _username = $("#name_order_address").val();
        var _phonenumber = $("#phone_order_address").val();
        var _province = $("#province_order_address").val();
        var _district = $("#district_order_address").val();
        var _ward = $("#ward_order_address").val();
        var _address_content = $("#order_address_content").val();
        if (_username == "" || _phonenumber == "" || _province == "" || _district == "" || _ward == "" || _address_content == "") {
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
                title: 'Nhập thông tin còn thiếu'
            })
            return false;
        }
        else if (_phonenumber.length < 10 || _phonenumber.length > 10) {
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
                title: 'Số điện thoại phải đúng 10 chữ số'
            })
            return false;
        }
        else if (_username.length > 20) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2000,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Họ tên không quá 20 ký tự'
            })
        }
        else if (_address_content.length > 50) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2000,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: 'Địa chỉ cụ thể không quá 50 ký tự'
            })
        }
        else{
        $.ajax({
            type: "POST",
            url: '/Account/ChangeOrderAddress',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: ide, username: _username, phonenumber: _phonenumber, province: _province, district: _district, ward: _ward,address_content: _address_content}),
            dataType: "json",
            success: function (result) {
                editModal2.modal('hide');
                if (result == true) {
                    $(".update_user_name").text(_username);
                    $(".update_phone").text("Điện thoại: " +_phonenumber);
                    $(".update_address").text("Địa chỉ: " + _address_content + ', ' + _ward + ', ' + _district + ', ' + _province);
                    $(".remove_adress").remove();
                    const Toast = Swal.mixin({
                        toast: true,
                        position: 'top',
                        showConfirmButton: false,
                        timer: 2000,
                        didOpen: (toast) => {
                            toast.addEventListener('mouseenter', Swal.stopTimer)
                            toast.addEventListener('mouseleave', Swal.resumeTimer)
                        }
                    })
                    Toast.fire({
                        icon: 'success',
                        title: 'Sửa địa chỉ thành công'
                    })
                }
                else {
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
                        icon: 'error',
                        title: 'Lỗi'
                    })
                }
            }
        })
        }
    });
}