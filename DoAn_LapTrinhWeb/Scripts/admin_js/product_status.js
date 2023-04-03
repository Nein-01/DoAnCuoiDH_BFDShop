var changeStt = function (xthis) {
	var xid = xthis.id;
    var st = xthis.checked;
    $.ajax({
        type: "POST",
		url: '/ProductsAdmin/ChangeStatus',
        contentType: "application/json; charset=utf-8",
		data: JSON.stringify({ id: xid, state: st }),
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
					title: 'Bạn không có quyền đổi trạng thái'
				})
            }
        },
		error: function () {
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
				title: 'Thay đổi trạng thái không thành công'
			})
		}
    });
}