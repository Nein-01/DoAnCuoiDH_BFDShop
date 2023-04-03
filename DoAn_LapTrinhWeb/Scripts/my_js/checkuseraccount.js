//sử dụng "sweetalert2" để hiện các thông báo toast, thư viện được đã đuọc thêm vào main_layout  " <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.all.min.js"></script>"

//kiểm tra đăng nhập
var SignIn = function () {
    var username = $("#Email").val();
    var password = $("#Pass").val();
    if (username == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2500,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập Email đăng nhập'
        })
        return false;
    }
    //check rỗng password
    if (password == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2500,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập password'
        })
        return false;
    }
    var data = $("#loginForm").serialize();
    $.ajax({
        type: "POST",
        url: "/Account/SignIn", //kiểm tra tồn tại username, username và password đã trùng chưa (kiểm tra ở acition checksignin của cotroller Account)
        data: data,
        success: function (result) {
            if (result == "Disable") {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Tài khoản bị vô hiệu hóa'
                })
                $("#loginForm")[0].reset();
            }
            else
            if (result == "NonActivated") {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2500,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Tài khoản chưa được kích hoạt'
                })
                $("#loginForm")[0].reset();
            }
            else
                if (result == "Success") {
                    window.location.reload();
                //window.location.href = "/Home/Index";
            } else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Tài khoản hoặc mật khẩu không đúng'
                })
                $("#loginForm")[0].reset();
            }
        }
    })
}

//Quên mật khẩu
function ForgotPass() {
    var email = $("#Emailforgot").val();
    if (email == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 2500,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Vui lòng nhập Email'
        })
        return false;
    }

}

//Xác thực tài khoản
function Verified() {
    $.ajax({
        type: "post",
        url: "/Account/RegisterConfirm",
        data: { 'regId': '@ViewBag.regID' },
        success: function (msg) {
            $("#previous").hide();
            $("#after").show();
            alert(msg);
        }
    })
}
