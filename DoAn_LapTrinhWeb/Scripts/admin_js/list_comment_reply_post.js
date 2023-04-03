//Xem nội dung bình luận cha
var ViewCmtModal = $('#ViewCommentModal');
var View__cmt = function (cmt_id, comment_content) {
    ViewCmtModal.find('.summernote_view_cmt').html(comment_content)
    ViewCmtModal.modal('show');
}
//Xem nội dung bình luận con
var ViewRepCmtModal = $('#ViewRepCommentModal');
var View_rep_cmt = function (rep_cmt_id, rep_comment_content) {
    ViewRepCmtModal.find('.summernote_view_rep_cmt').val(rep_comment_content)
    ViewRepCmtModal.modal('show');
}

$('.close_modal').click(function(){
    ViewCmtModal.modal('hide');
    ViewRepCmtModal.modal('hide');
})
//Trả lời bình luận
var ReplyModal = $('#ReplyCommentModal');
var Replycom__mt = function (cmt_id, comment_content, post) {
    _cmt = cmt_id
    $('#comment_content').html(comment_content);
    $('#Post').val(post);
    ReplyModal.modal('show');
    $('.summernote_cmt').summernote({
        height: 360,
        toolbar: false,
    });
    $('.summernote_rep_cmt').summernote({
        height: 300,
        placeholder: 'Nhập nội dung phản hồi'
    });
    $('.note-toolbar .note-table,.note-toolbar .note-color,.note-toolbar .note-fontname,.note-toolbar .note-font,.note-toolbar .note-icon-question, .note-toolbar .note-style:first, .note-toolbar .note-para').remove();
}
$('#ConfirmReplyComment').click(function () {
    var _reply_cmt = $('#ReplyCom_Conent').val();
    if (_reply_cmt == "") {
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
            title: 'Vui lòng nhập đầy đủ thông tin'
        })
    }
    else {
        $.ajax({
            type: "post",
            url: '/CommentReply/ReplyComment',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ comment_id: _cmt, reply_comment_content: _reply_cmt  }),
            dataType: "json",
            success: function (result) {
                ReplyModal.modal('hide');
                if (result == true) {
                    $("#dis_rep_" + _cmt + "").removeAttr("onclick");
                    $("#dis_rep_" + _cmt + "").addClass("cursor-disable");
                    $("#dis_rep_" + _cmt + "").removeClass("btn-bg-light");
                    $("#dis_rep_" + _cmt + "").removeClass("btn-active-color-danger");

                    $("#dis_del_" + _cmt + "").removeAttr("onclick");
                    $("#dis_del_" + _cmt + "").addClass("cursor-disable");
                    $("#dis_del_" + _cmt + "").removeClass("btn-bg-light");
                    $("#dis_del_" + _cmt + "").removeClass("btn-active-color-danger");

                    $("#change_stats_" + _cmt + "").addClass("btn-success");
                    $("#change_stats_" + _cmt + "").removeClass("btn-secondary")
                    $("#change_stats_" + _cmt + "").removeAttr("onclick")

                    $("#waiting_stats_" + _cmt + "").addClass("btn-secondary");
                    $("#waiting_stats_" + _cmt + "").removeClass("btn-warning")
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
                        title: 'Phản hồi thành công'
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
                    icon: 'warning',
                    title: 'Lỗi'
                })
            }
        });
    }

});
//duyệt hàng loạt
var approved = $('#Modalapproved');
$('#BtnOpenApprovedCmt').click(function () {
    approved.modal('show');
})
$('#BtnApproved').click(function () {
    $.ajax({
        type: "get",
        url: '/CommentReply/ApprovedAllComment',
        success: function (result) {
            approved.modal('hide');
            if (result == true) {
                $('.approvedall').addClass('btn-success');
                $('.approvedall').removeClass('btn-secondary');
                $('.disable_waiting').addClass('btn-secondary');
                $('.disable_waiting').attr('disabled');
                $('.disable_waiting').removeClass('btn-warning');
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
                    title: 'Đã duyệt tất cả bình luận'
                })
            }
            else {
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
                    title: 'Không có bình luận nào cần duyệt'
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
});
//accept bình luận
var ApcceptModel = $('#IDAcceptModal');
var Accept_Cmt = function (id) {
    idcom = id
    ApcceptModel.find('.cmmt__id').text(id);
    ApcceptModel.modal('show');
}

$('#Accecpt_cmnt_btn').click(function () {
    $.ajax({
        type: "POST",
        url: '/CommentReply/AcceptComment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ comment_id: idcom }),
        dataType: "json",
        success: function (result) {
            ApcceptModel.modal('hide');
            if (result == true) {
                $('#change_stats_' + idcom + '').addClass('btn-success');
                $('#change_stats_' + idcom + '').removeClass('btn-secondary');
                $('#waiting_stats_' + idcom + '').addClass('btn-secondary');
                $('#waiting_stats_' + idcom + '').attr('disabled');
                $('#waiting_stats_' + idcom + '').removeClass('btn-warning');
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
                    icon: 'success',
                    title: 'Duyệt bình luận '+idcom+' thành công'
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
                icon: 'error',
                title: '!Lỗi'
            })
        }
    });
});
//xóa comment
var DeleteModal = $('#DeleteCommentModal');
var Delete_Cmt = function (id) {
    _idcom = id
    DeleteModal.find('.del_cmmt_id').text(id);
    DeleteModal.modal('show');
}
$('#del_cmt_confirm_btn').click(function () {
    $.ajax({
        type: "POST",
        url: '/CommentReply/DeleteComment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ del_comment_id: _idcom }),
        dataType: "json",
        success: function (result) {
            DeleteModal.modal('hide');
            if (result == true) {
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
                    title: 'Bạn không có quyền xóa'
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
//duyệt reply comment hàng loạt
var replyapproved = $('#RepModalapproved');
$('#BtnRepOpenApprovedCmt').click(function () {
    replyapproved.modal('show');
})
$('#BtnRepApproved').click(function () {
    $.ajax({
        type: "get",
        url: '/CommentReply/ApprovedAllReplyComment',
        success: function (result) {
            replyapproved.modal('hide');
            if (result == true) {
                $('.approvedall').addClass('btn-success');
                $('.approvedall').removeClass('btn-secondary');
                $('.disable_waiting').addClass('btn-secondary');
                $('.disable_waiting').attr('disabled');
                $('.disable_waiting').removeClass('btn-warning');
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
                    title: 'Đã duyệt tất cả bình luận'
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
});
//accpet reply comment
var ApcceptRepModal = $('#IDAccepReptModal');
var Accept_Rep_Cmt = function (id) {
    idcom = id
    ApcceptRepModal.find('.cmt_rep_id').text(id);
    ApcceptRepModal.modal('show');
}
$('#Accept_Rep_cmt_btn').click(function () {
    $.ajax({
        type: "POST",
        url: '/CommentReply/AcceptReplyComment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ rep_comment_id: idcom }),
        dataType: "json",
        success: function (result) {
            ApcceptRepModal.modal('hide');
            if (result == true) {
                $('#change_stats_' + idcom + '').addClass('btn-success');
                $('#change_stats_' + idcom + '').removeClass('btn-secondary');
                $('#change_stats_' + idcom + '').removeAttr('onclick');
                $('#waiting_stats_' + idcom + '').addClass('btn-secondary');
                $('#waiting_stats_' + idcom + '').attr('disabled');
                $('#waiting_stats_' + idcom + '').removeClass('btn-warning');
                $("#del_rep_" + idcom + "").removeClass("btn-bg-light");
                $('#del_rep_' + idcom + '').removeClass('btn-active-color-danger');
                $('#del_rep_' + idcom + '').removeAttr('onclick');
                $('#del_rep_' + idcom + '').addClass('cursor-disable');
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
                    icon: 'success',
                    title: 'Duyệt bình luận ' + idcom + ' thành công'
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
                icon: 'error',
                title: '!Lỗi'
            })
        }
    });
});
//xóa reply comment
var DeleteRepModal = $('#DeleteRepCommentModal');
var Delete_Rep_Cmt = function (id) {
    _idrepcmt = id
    DeleteRepModal.find('.del_rep_cmmt_id').text(id);
    DeleteRepModal.modal('show');
}
$('#delrep_cmt_confirm_btn').click(function () {
    $.ajax({
        type: "POST",
        url: '/CommentReply/DeleteRepComment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ del_rep_comment_id: _idrepcmt }),
        dataType: "json",
        success: function (result) {
            DeleteRepModal.modal('hide');
            if (result == true) {
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
                    title: 'Bạn không có quyền xóa'
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

