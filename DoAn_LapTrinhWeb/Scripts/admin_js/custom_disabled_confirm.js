$('#disable_confirm').on('click', function (e) {
    e.preventDefault();
    var self = $(this);
    console.log(self.data('title'));
    Swal.fire({
        title: 'Vô hiệu hóa?',
        text: "Nhấn đồng ý để vô hiệu xóa và hủy để quay lại",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý!',
        cancelButtonText: 'Hủy!',
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                'Vô hiệu hóa thành công!',
                'Mục đã chọn đã được chuyển vào thùng rác',
                'success'
            )
            location.href = self.attr('href');
        }
    })
})
