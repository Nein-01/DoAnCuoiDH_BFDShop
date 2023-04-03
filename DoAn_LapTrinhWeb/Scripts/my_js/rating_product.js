var newModal = $('#addModal');
$('#button_add_rating').click(function () {
    newModal.modal('show');
});
$('#ConfirmAdd').click(function () {
    newModal.modal('hide');
});
$('#modelclose').click(function () {
    newModal.modal('hide');
})
//phần chọn sao đánh giá
//di chuột ra khỏi đối tượng
function CRateOut(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star-o');
    }
}
//di chuột vào một đối tượng
function CRateOver(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');
    }
}
//click vào đối tượng
function CRateClick(rating) {
    $('.uploadimgcheck').removeAttr('disabled');
    $('.write_content_fb').removeClass('cursor-disable');
    $('#ConfirmAdd').removeAttr('disabled');
    $('#FBk_Content').removeAttr('disabled');
    $("#dcript_content_fb").css("color", "#666");
    $("#dcript_content_fb").text("Nhập nội dung đánh giá");
    $("#lblRating").val(rating);
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');
       
    }
    for (var i = 1; i <= 5; i++) {
        $("#rate" + i).attr('class', 'fa fa-star-o');
    }
}
//di chuột ra khỏi đối tượng
function CRateSelected() {
    var rating = $("#lblRating").val();
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');
    }
}
//phải login trước khi bình luận
$(".rating_login").click(function () {
    var self = $(this);
    $.get("/account/userlogged", {},//ajax
        function (isLogged, textStatus, jqXHR) {
            if (!isLogged) {
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
                    icon: 'error',
                    title: 'Yêu cầu đăng nhập?'
                })
            }
            else {
                location.href = ev.currentTarget.href;
            }
        },
        "json"
    );
});
//set số lượn upload ảnh, và kích thước file
$('#uploadimgrate').change(function () {
    $(".btn_uploadimgrating").text("Chọn lại");
    var files = $('.uploadimgcheck')[0].files;
    if (files.length > 5)//số lượng file tối đa được tải lên
    {
        $(".show_error_limit_img").text("(Bạn chỉ được chọn tối đa 5 ảnh)");
        $(".preimg_rate").attr("hidden");
        $('#uploadimgrate').val('');
        return false;//reset file ảnh
    }
    else
    {
        $(".show_error_limit_img").text("");
        $(".preimg_rate").removeAttr("hidden");
    }
    if (files.size > 2000000)//đơn vị tính 2000000KB = > kích thước ảnh tối đa 2MB
    {
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
            title: 'Kích thước ảnh phải nhỏ hơn 2MB,vui lòng chọn lại'
        })
        $('#uploadimgrate').val('');
        return false;
    }
});
//xem trước ảnh trước khi gửi đánh giá
let fileInput = document.getElementById("uploadimgrate");
let imageContainer = document.getElementById("pre_img_rate");
function preview() {
    imageContainer.innerHTML = "";
    for (i of fileInput.files)//up list ảnh
    {
        if (fileInput.files.length <= 5) {
            let reader = new FileReader();
            let figure = document.createElement("figure");
            let figCap = document.createElement("figcaption");
            figure.appendChild(figCap);
            reader.onload = () => {
                let img = document.createElement("img");
                img.setAttribute("src", reader.result);
                figure.insertBefore(img, figCap);
            }
            imageContainer.appendChild(figure);
            reader.readAsDataURL(i);//load ảnh lên view(ảnh được load lên dạng base64)
        }
    }
}
//đếm text nội dung
$('textarea').keyup(function () {
    var characterCount = $(this).val().length,
        current = $('#current'),
        maximum = $('#maximum'),
        theCount = $('#the-count');
    current.text(characterCount);
    if (characterCount < 70) {
        $("#current").css("color", "#666");
        $("#dcript_content_fb").css("color", "#666");
        $("#dcript_content_fb").text("Nội dung tối thiểu 70 ký tự");
    }
    else if (characterCount >= 70 && characterCount <= 150) {
        $("#current").css("color", "#47c90e");
        $("#dcript_content_fb").css("color", "#47c90e");
        $("#dcript_content_fb").text("Nội dung tối thiểu 70 ký tự");
    }
    else if (characterCount > 150 && characterCount < 199) {
        $("#current").css("color", "#ff9900");
        $("#dcript_content_fb").css("color", "#ff9900");
        $("#dcript_content_fb").text("Nội dung tối thiểu 70 ký tự");
    }
    else if (characterCount >= 200) {
        $("#maximum").css("color", "#8f0001");
        $("#current").css("color", "#8f0001");
        $("#dcript_content_fb").css("color", "#8f0001");
        $("#current").css("font-family", "Roboto-Medium");
        $("#maximum").css("font-family", "Roboto-Medium"); 
        $("#dcript_content_fb").text("Nội dung đã đạt đến giới hạn");
    } else {
        $("#maximum").css("color", "#666");
    }
});

//đánh giá sản phẩm từ trang chi tiết đơn hàng
var OdRating = function (order_id,product_id, gen_id) {
    newModal.modal('show');
    $('#ProD_ID').val(product_id);
    $('#Genre_ID').val(gen_id);
    //xử lý lưu dữ liệu database
    od_id = order_id
}
//xử lý lưu dữ liệu database,cái này dành cho tracking_order_detail(dữ liệu được xử lý ngầm trên view và controller)
function AjaxPost2(formData)//get data trong onsubmit="AjaxPost(this)"
{
    $.ajax({
        type: 'post',
        url: '/Products/RatingProduct',//gọi đến action xử lý lưu dữ liệu xuống database
        data: new FormData(formData),
        contentType: false,
        processData: false,
        success: function () {
            location.href = od_id//phải có id thì nó mới redirect tới trang chi tiết đơn hàng
        }
    })
}
//xử lý lưu dữ liệu database,dùng cho product detail(dữ liệu được xử lý ngầm trên view và controller)
function AjaxPost(formData)//get data trong onsubmit="AjaxPost(this)"
{
    $.ajax({
        type: 'post',
        url: '/Products/RatingProduct',
        data: new FormData(formData),
        contentType: false,
        processData: false,
        success: function () {
        }
    })
}
