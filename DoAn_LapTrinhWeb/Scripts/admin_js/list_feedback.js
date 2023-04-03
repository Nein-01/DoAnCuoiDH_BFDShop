var editModal = $('#editModal10');
var ide;
var ReplyFB = function (id,productname,productid,genre,rating,fbcontent) {
    $('#ProductName').val(productname);
    $('#ProductID').val(productid);
    $('#Star').val(rating);
    $('#GenID').val(genre);
    $('#FBContent').val(fbcontent);
    $('#ParenFBID').val(id);
    editModal.modal('show');
    $("#ReplyConent")[0].reset();
}
$('#ConfirmReply').click(function () {
    var _fbreply = $('#ReplyConent').val();
    var _genreid = $('#GenID').val();
    var _parentfbid = $('#ParenFBID').val();
    var _productid = $('#ProductID').val();
    if (_fbreply == "") {
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
            title: 'Hãy nhập đầy đủ thông tin'
        })
    }
    else {
        $.ajax({
            type: "post",
            url: '/Feedbacks/ReplyFeedback',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ feedback_content: _fbreply, parent_feedback_id: _parentfbid, genre_id: _genreid, product_id:_productid }),
            dataType: "json",
            success: function (result) {
                editModal.modal('hide');
                if (result == true) {
                    $("#btn_reply_fb").removeAttr("onclick");
                    $("#btn_reply_fb").addClass("cursor-disable");
                    $("#btn_reply_fb").removeClass("btn-bg-light");
                    $("#btn_reply_fb").removeClass("btn-active-color-danger");
                    $("#tooltop_replyfb").attr("data-bs-original-title","Đã phản hồi đánh giá");
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
                        title: 'Phản hồi đánh giá id ' + _parentfbid+' thành công'
                    })
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
                    title: 'Lỗi'
                })
            }
        });
    }

});
////

var delmodal = $('#CancleModal');
var CancleConfirm = function (id, fbcontent) {
    delmodal.find('.lable_feedback').text(id);
    delmodal.find('.fbcontent').text(fbcontent);
    delmodal.modal('show');
    idde = id;
}
$('#CancleBtn').click(function () {
    $.ajax({
        type: "POST",
        url: '/Feedbacks/CancleFeedback',
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
                    title: 'Bạn không có quyền chỉnh sửa hoặc đánh giá đã được duyệt'
                })
            }
            else {
                delmodal.modal('hide');
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





