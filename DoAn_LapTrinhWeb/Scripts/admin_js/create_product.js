var addgenreModal = $('#AddGenreModal');
var addbrandModal = $('#AddBrandModal');
var adddiscountModal = $('#AddDiscountModal');
var addtaxesModal = $('#AddTaxesModal');
//thêm nhanh discount
$('#AddDiscountModal').click(function () {
    var data = $("#form_discount_create").serialize();
    var dicount_name = $("#discount_name_add").val();
    var dicount_price = $("#discount_price_add").val();
    var dicount_start = $(".discount_start_add").val();
    var dicount_end = $(".discount_end_add").val();
    if (dicount_name == "") {
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
            icon: 'error',
            title: 'Nhập tên chương trình'
        })
    }
    else if (dicount_price == "") {
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
            icon: 'error',
            title: 'Nhập mức giảm'
        })
    }
    else if (dicount_start == "") {
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
            icon: 'error',
            title: 'Chọn ngày bắt đầu'
        })
    }
    else if (dicount_end == "") {
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
            icon: 'error',
            title: 'Chọn ngày kết thúc'
        })
    }
    else {
    $.ajax({
        type: "POST",
        url: '/ProductsAdmin/CreateDiscounts',
        data: data,
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
                title: 'Thêm thất bại'
            })
        }
    });
    }
});

//thêm mới brand
$('#AddBrandModal').click(function () {
    var data = $("#form_brand_create").serialize();
    var brand_name = $("#brand_name_add").val();
    if (brand_name == "") {
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
            icon: 'error',
            title: 'Nhập tên thương hiệu'
        })
    }
    else {
        $.ajax({
            type: "POST",
            url: '/ProductsAdmin/CreateBrands',
            data: data,
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
                    title: 'Thêm thất bại'
                })
            }
        });
    }
});
//thêm mới genre
$('#AddGenreModal').click(function () {
    var data = $("#form_genre_create").serialize();
    var genre_name = $("#genre_name_create").val();
    var paren_genre_name = $("#parent_gen_id_create").val();
    if (paren_genre_name == "") {
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
            icon: 'error',
            title: 'Chọn thể loại cha'
        })
    }
    else if (genre_name == "") {
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
            icon: 'error',
            title: 'Nhập tên thể loại'
        })
    }
    else {
        $.ajax({
            type: "POST",
            url: '/ProductsAdmin/CreateGenres',
            data: data,
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
                    title: 'Thêm thất bại'
                })
            }
        });
    }
});
//thêm mới vat
$('#AddTaxesModal').click(function () {
    var data = $("#form_taxes_create").serialize();
    var taxes_name = $("#taxes_name_create").val();
    var taxes_value = $("#taxes_value_create").val();
    if (taxes_name == "") {
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
            icon: 'error',
            title: 'Nhập tên VAT'
        })
    }
    else if (taxes_value == "") {
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
            icon: 'error',
            title: 'Nhập phần trăm VAT'
        })
    }
    else {
        $.ajax({
            type: "POST",
            url: '/ProductsAdmin/CreateTaxes',
            data: data,
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
                    title: 'Thêm thất bại'
                })
            }
        });
    }
});



