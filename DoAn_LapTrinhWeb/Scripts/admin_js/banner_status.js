var changeStt = function (xthis) {
	var xid = xthis.id;
    var st = xthis.checked;
    $.ajax({
        type: "POST",
		url: '/Banners/ChangeStatus',
        contentType: "application/json; charset=utf-8",
		data: JSON.stringify({ id: xid, state: st }),
        dataType: "json",
        success: function (recData) {
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
				title: recData.Message
			})
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