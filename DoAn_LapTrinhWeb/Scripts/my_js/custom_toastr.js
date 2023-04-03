$(window).on('load', function () {
    const mgs_type = $('input[name=mgs_type]').val();
    const msg = $('input[name=mgs]').val();
    //
    const mgs_type1s = $('input[name=mgs_type1s]').val();
    const msg1s = $('input[name=mgs1s]').val();
    //notifi 1.5s
    if (mgs_type1s != '' && msg1s != '') {
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
        switch (mgs_type1s) {
            case 'success':
                Toast.fire({
                    icon: 'success',
                    title: msg1s
                })
                break;
            case 'info':
                Toast.fire({
                    icon: 'info',
                    title: msg1s
                })
            case 'warning':
                Toast.fire({
                    icon: 'warning',
                    title: msg1s
                })
            case 'danger':
                Toast.fire({
                    icon: 'error',
                    title: msg1s
                })
        }
    }
    //notifi 2.5s
    if (mgs_type != '' && msg != '') {
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
        switch (mgs_type) {
            case 'success':
                Toast.fire({
                    icon: 'success',
                    title: msg
                })
                break;
            case 'info':
                Toast.fire({
                    icon: 'info',
                    title: msg
                })
            case 'warning':
                Toast.fire({
                    icon: 'warning',
                    title: msg
                })
            case 'danger':
                Toast.fire({
                    icon: 'error',
                    title: msg
                })
        }
    }
});

// 404 không gọi được
// 500 lời gọi sai ajax
