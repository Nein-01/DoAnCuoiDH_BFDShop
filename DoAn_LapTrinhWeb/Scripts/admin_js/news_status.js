var changeStt = function (xthis) {
	var xid = xthis.id;
    var st = xthis.checked;
    $.ajax({
        type: "POST",
		url: '/NewsAdmin/ChangeStatus',
        contentType: "application/json; charset=utf-8",
		data: JSON.stringify({ id: xid, state: st }),
        dataType: "json",
		success: function (result) {
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
					timer: 1500,
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
    });
}