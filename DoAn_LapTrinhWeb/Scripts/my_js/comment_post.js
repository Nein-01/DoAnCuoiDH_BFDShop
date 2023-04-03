$(document).ready(function () {
    $('.form_comment_post').summernote({
        height: 100,
        placeholder: 'Để lại bình luận của bạn tại đây...',
    });
    //set tên thôi
    $('.note-icon-picture').click(function () {
        $('.note-image-btn').val('Chèn');
        $('.modal-title').text('Chèn ảnh');
        $('.note-group-select-from-files .note-form-label').text('Chọn từ File');
        $('.note-group-image-url .note-form-label').text('URL ảnh');
    })
    //set tên
    $('.note-icon-link').click(function () {
        $('.note-link-btn').val('Chèn');
        $('.modal-title').text('Chèn Link');
    })
    $('.close').click(function () {
        $('.note-modal').modal('hide');
    })
    //các chức năng trong toolbar cần tắt
    $('.note-toolbar .note-table,.note-toolbar .note-color,.note-toolbar .note-fontname,.note-toolbar .note-font,.note-toolbar .note-icon-question, .note-toolbar .note-style:first, .note-toolbar .note-para, .note-toolbar .note-view').remove();
    //phải login trước khi bình luận bài viết
    $(".note-editing-area,.request_login").click(function (ev) {
        ev.preventDefault();
        $('#create_submit_comment').removeAttr('hidden');
        $.get("/account/userlogged", {},
            function (isLogged, textStatus, jqXHR) {
                if (!isLogged) {
                    //gọi action đăng nhập khi người dùng bấm thanh toán mà chưa đăng nhập hệ thống
                    bootbox.confirm({
                        message: "Vui lòng đăng nhập để thực hiện chức năng này!",
                        buttons: {
                            confirm: {
                                label: 'Đăng nhập',
                                className: 'btn-info'
                            },
                            cancel: {
                                label: 'Quay lại',
                                className: 'btn-secondary'
                            }
                        },
                        callback: function (result) {
                            if (result) {
                                window.location = "/Account/SignIn";
                            }
                        }
                    });
                }
            },
            "json"
        );
    });
    //thêm bình luận
    $('#create_submit_comment').click(function () {
        var com_content = $("#comment__con").val();
        if (com_content == "") {
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
                title: 'Vui lòng nhập nội dung bình luận'
            })
            return false;
        }
        else if (com_content.length < 20) {
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
                title: 'Nội dung bình luận tối thiểu 20 ký tự'
            })
            return false;
        }
        else if (com_content.length > 500) {
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
                title: 'Nội dung bình luận không quá 500 ký tự'
            })
            return false;
        }
        else {
            var data = $("#create_comment_post").serialize();
            $.ajax({
                type: "GET",
                url: "/News/CommentPost", //kiểm tra tồn tại username, username và password đã trùng chưa (kiểm tra ở acition checksignin của cotroller Account)
                data: data,
                success: function (result) {
                    if (result == true) {
                        var find_text = $('#create_comment_post')
                        find_text.find('.note-editable').text('')
                        $('#create_comment_post').val('')
                        $('.append__cmt_create').removeAttr('hidden')
                        $('.append__cmt_create').text('Bình luận của bạn đang chờ xét duyệt và sẽ hiển thị sau khi được duyệt thành công.')
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
                            title: 'Bình luận thành công'
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
                            title: 'Lỗi'
                        })
                    }
                }
            })
        };
    });
});
//parent reply comment post
var Create_Reply_comm = function (id, acc_name) {
    ide = id;
    $('.append__cmt').text('')
    $('#areas_reply_comment_' + id + '').collapse('show')
    $('#areas_reply_comment_'+id+'').collapse('hide')
    $('#submit_reply_comm_' +id+'').removeAttr('hidden');
    $('#areas_reply_comment_'+id+'').removeAttr('hidden');
    $('#areas_reply_comment_' + id + '').addClass('mt-2');
    $('#reply_comment_con_' + id + '').text("@" + acc_name + ":");
    $('#reply_comment_con_' + id + '').summernote({
        height: 100,
        placeholder: 'Để lại bình luận của bạn tại đây...',
        hint: {
            words: [acc_name],
            match: /\b(\w{1,})$/,
            search: function (keyword, callback) {
                callback($.grep(this.words, function (item) {
                    return item.indexOf(keyword) === 0;
                }));
            }
        }
    });
    //ản các chức năng trong toolbar
    $('.note-toolbar .note-table,.note-toolbar .note-color,.note-toolbar .note-fontname,.note-toolbar .note-font,.note-toolbar .note-icon-question, .note-toolbar .note-style:first, .note-toolbar .note-para, .note-toolbar .note-view').remove();
    //gửi reply bình luận
    $('#submit_reply_comm_' + ide + '').click(function () {
        var _reply_content = $('#reply_comment_con_' + ide + '').val();
        if (_reply_content == "") {
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
                title: 'Vui lòng nhập nội dung bình luận!'
            })
        }
        else if (_reply_content.length < 20) {
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
                title: 'Nội dung bình luận tối thiểu 20 ký tự'
            })
            return false;
        }
        else if (_reply_content.length > 500) {
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
                title: 'Nội dung bình luận không quá 500 ký tự'
            })
            return false;
        }
        else {
            $.ajax({
                type: "POST",
                url: '/News/ReplyCommentPost',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ id: ide, reply_content: _reply_content }),
                dataType: "json",
                success: function (result) {
                    var find_text = $('#find_rep_parent_content_' + ide + '')
                    find_text.find('.note-editable').text("@" + acc_name + ":")
                    $('#reply_comment_con_' + ide + '').val("@" + acc_name + ":")
                    $('#show_rep_parent_complete_' + ide + '').removeAttr('hidden')
                    $('#show_rep_parent_complete_' + ide + '').text('Bình luận của bạn đang chờ xét duyệt và sẽ hiển thị sau khi được duyệt thành công.')
                    if (result == true) {
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
                            title: 'Bình luận thành công'
                        })
                    }
                    else {
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
                        icon: 'danger',
                        title: 'Lỗi'
                    })
                }
            });
        }
    });
}
//child reply comment post
var Create_Child_Reply_comm = function (id,reply_id, acc_name) {
    child_ide = id;
    child_rep = reply_id
    $('.append__cmt').text('')
    $('#areas_child_reply_comment_' + reply_id + '').collapse('show')
    $('#areas_reply_comment_'+id+'').collapse('hide')
    $('#areas_child_reply_comment_'+reply_id+'').collapse('hide')
    $('#submit_child_reply_comm_' +reply_id+'').removeAttr('hidden');
    $('#areas_child_reply_comment_'+reply_id+'').removeAttr('hidden');
    $('#areas_child_reply_comment_'+reply_id+'').addClass('mt-2');
    $('#child_reply_comment_con_' + reply_id + '').text("@" + acc_name + ":");
    $('#child_reply_comment_con_'+reply_id+'').summernote({
        height: 100,
        placeholder: 'Để lại bình luận của bạn tại đây...',
        hint: {
            words: [acc_name],
            match: /\b(\w{1,})$/,
            search: function (keyword, callback) {
                callback($.grep(this.words, function (item) {
                    return item.indexOf(keyword) === 0;
                }));
            }
        }
    });
    //ản các chức năng trong toolbar
    $('.note-toolbar .note-table,.note-toolbar .note-color,.note-toolbar .note-fontname,.note-toolbar .note-font,.note-toolbar .note-icon-question, .note-toolbar .note-style:first, .note-toolbar .note-para, .note-toolbar .note-view').remove();
    //gửi reply bình luận
    $('#submit_child_reply_comm_' + child_rep + '').click(function () {
        var _child_reply_content = $('#child_reply_comment_con_' + child_rep + '').val();
        if (_child_reply_content == "") {
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
                title: 'Vui lòng nhập nội dung bình luận!'
            })
        }
        else if (_child_reply_content.length < 20) {
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
                title: 'Nội dung bình luận tối thiểu 20 ký tự'
            })
            return false;
        }
        else if (_child_reply_content.length > 500) {
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
                title: 'Nội dung bình luận không quá 500 ký tự'
            })
            return false;
        }
        else {
            $.ajax({
                type: "POST",
                url: '/News/ReplyCommentPost',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ id: child_ide, reply_content: _child_reply_content }),
                dataType: "json",
                success: function (result) {
                    if (result == true) {
                        $('#rep_for_' + child_rep + '').removeAttr('hidden')
                        $('#reply_avatar_' + child_rep + '').removeAttr('hidden')
                        var set_text_reply = $('#find_content_' + child_rep + ' .note-editable').text()
                        $('#content__rep_' + child_rep + '').text(set_text_reply)


                        $('#show_rep_complete_' + child_rep + '').removeAttr('hidden')
                        var find_text = $('#find_content_' + child_rep + '')
                        find_text.find('.note-editable').text("@" + acc_name + ":")
                        $('#child_reply_comment_con_' + child_rep + '').val("@" + acc_name + ":")
                        $('#show_rep_complete_' + child_rep+'').text('Bình luận của bạn đang chờ xét duyệt và sẽ hiển thị sau khi được duyệt thành công.')
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
                            title: 'Bình luận thành công'
                        })
                    }
                    else {
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
                        icon: 'danger',
                        title: 'Lỗi'
                    })
                }
            });
        }
    });
}
//reaction comment post
var Like_Comment = function (cmt_id, count_like_cmt) {
    var id = cmt_id
    var count_like = count_like_cmt
    $.ajax({
        type: "POST",
        url: '/News/ReactionCommmentPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ comment_id: id}),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReacton_' + id + '').attr('onclick', 'Remove_Like_Comment(' + id + ',' + (count_like + 1)+ ')' + '');
                $('#BtnReacton_' + id + '').text('Bỏ thích');
                $('#sumlike_cmt_' + id + '').text('('+(count_like+1)+')');
            }
            else {
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
                icon: 'danger',
                title: 'Lỗi'
            })
        }
    });
}
// remove reaction comment post
var Remove_Like_Comment = function (cmt_id, count_like_cmt) {
    var id = cmt_id
    var count_like = count_like_cmt
    $.ajax({
        type: "POST",
        url: '/News/ReactionCommmentPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ comment_id: id }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReacton_' + id + '').attr('onclick', 'Like_Comment(' + id + ',' + (count_like - 1) + ')' + '');
                $('#BtnReacton_' + id + '').text('Thích');
                $('#sumlike_cmt_' + id + '').text('('+(count_like - 1)+')');
            }
            else {
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
                icon: 'danger',
                title: 'Lỗi'
            })
        }
    });
}
//reaction reply comment post
var Rely_Like_Comment = function (rep_cmt_id, count_rep_like_cmt) {
    var rep_id = rep_cmt_id
    var count_like_rep = count_rep_like_cmt
    $.ajax({
        type: "POST",
        url: '/News/ReactionReplyCommment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ rep_comment_id: rep_id }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReplyReacton_' + rep_id + '').attr('onclick', 'Remove_Reply_Like_Comment(' + rep_id + ',' + (count_like_rep + 1) + ')' + '');
                $('#BtnReplyReacton_' + rep_id + '').text('Bỏ thích');
                $('#sum_reply_like_cmt_' + rep_id + '').text('(' + (count_like_rep + 1) + ')');
            }
            else {
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
                icon: 'danger',
                title: 'Lỗi'
            })
        }
    });
}
// remove reply reaction comment post
var Remove_Reply_Like_Comment = function (rep_cmt_id, count_rep_like_cmt) {
    var rep_id = rep_cmt_id
    var count_rep_like = count_rep_like_cmt
    $.ajax({
        type: "POST",
        url: '/News/ReactionReplyCommment',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ rep_comment_id: rep_id }),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#BtnReplyReacton_' + rep_id + '').attr('onclick', 'Rely_Like_Comment(' + rep_id + ',' + (count_rep_like - 1) + ')' + '');
                $('#BtnReplyReacton_' + rep_id + '').text('Thích');
                $('#sum_reply_like_cmt_' + rep_id + '').text('(' + (count_rep_like - 1) + ')');
            }
            else {
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
                icon: 'danger',
                title: 'Lỗi'
            })
        }
    });
}
