/*
 * Format giỏ hàng: product_{id}={quantity}
 */
// Khởi tạo giỏ hàng khi vào trang giỏ hàng
$(window).ready(function (ev) {
	$("#cartCount").text(Cookie.countWithPrefix("product"));
});
// Button xóa sản phẩm khỏi giỏ hàng
$(".js-delete").click(function (ev) {
	bootbox.confirm({
		message: "Xoá sản phẩm?",
		buttons: {
			confirm: {
				label: 'Xoá',
				className: 'btn-danger'
			},
			cancel: {
				label: 'Quay lại',
				className: 'btn-secondary'
			}
		},
		callback: function (result) {
			if (result) {
				var id = $(ev.currentTarget).data("id");// lấy id của sản phẩm
				var item = $(ev.currentTarget).closest(".item");
				item.remove();//xóa sản phẩm ở view giỏ hàng
				Cookie.remove("product_" + id);//xóa sản phẩm khỏi cookie
				var productCount = Cookie.countWithPrefix("product") // đếm số sản phẩm đã thêm
				$("#cartCount").text(productCount);//hiển thị số sản phẩm trong trang giỏ hàng
				$("#lblCartCount").text(productCount == 0 ? "0" : productCount);//số sản phẩm = 0  thì hiển thị = 0 và ngược lại
				updateOrderPrice();
			}
		}
	})
});
//cập nhật giỏ hảng
function updateOrderPrice() {
	var quanInputs = $("input.qty-input");//số lượng của sản phẩm ở ô input
	var newTotal = 0;
	var totalWithFee;
	quanInputs.each(function (i, e)
	{
		var price = Number($(e).data('price'));
		var quan = Number($(e).val());
		newTotal += price * quan;
	});
	var eleDiscount = $("#discount");
	var discount = 0;
	if (eleDiscount.attr("data-price") == null)
	{
		totalWithFee = newTotal + 30000;
	}
	else
	{
		if (eleDiscount.attr("data-price") < (newTotal))
		{
			discount = Number(eleDiscount.attr("data-price"));
			totalWithFee = newTotal + 30000 - discount;
		}
		else//nếu giảm giá lớn hơn tổng trị giá đơn hàng(tạm tính) thì tổng tiền bằng 30k(30k là phí ship chỉ trừ tiền sản phẩm chứ không trừ tiền ship)
		{
			discount = Number(eleDiscount.attr("data-price"));
			totalWithFee = 30000;
		}
	}
	totalWithFee += "";
	newTotal += "";
	discount += "";
	var regex = /(\d)(?=(\d{3})+(?!\d))/g;//covert giá sang dấu chấm
	$("#totalPrice").text(newTotal.replace(regex, "$1.") + "₫");//gắn tổng trị giá đơn hàng lên view
	$("#total_price").val(newTotal);
	$("#totalPriceWithFee").text(totalWithFee.replace(regex, "$1.") + "₫");
	$("#discount").text(discount.replace(regex, "$1.") + "₫");
};
//được sử dụng khi bấm nút thanh toán
$(".js-checkout").click(function (ev) {
	ev.preventDefault();
	var count_product = Cookie.countWithPrefix("product")
	$.get("/account/userlogged", {},
		function (isLogged, textStatus, jqXHR) {
			if (!isLogged) {
				//gọi action đăng nhập khi người dùng bấm thanh toán mà chưa đăng nhập hệ thống
				bootbox.confirm({
					message: "!Vui lòng đăng nhập để thực hiện chức năng thanh toán",
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
			else if (count_product == 0) {
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
					title: 'Giỏ hàng bạn chưa có sản phẩm nào!'
				})
            }
			else {
				location.href = ev.currentTarget.href;//target đến đường link mà bạn muốn đến "<a href="@Url.Action("Checkout","Cart")">Thanh toán</a>"
			}
		},
		"json"
	);
});
//Sử dụng code giảm giá
$(".btn-submitcoupon").click(function (ev) {
	var input = $(ev.currentTarget).prev();
	var _code = input.val().trim();
	var _totalprice = $("#total_price").val().trim();//get tổng tiền từ view đưa xuống controller để tính tiền giảm tối đa(áp dụng cho giảm giá theo%)
	var newTotal = 0;//cái này có sd nha
	var quanInputs = $("input.qty-input");//get sản phẩm từ input view giỏ hàng
	quanInputs.each(function (i, e) {
		var price = Number($(e).data('price'));
		var quan = Number($(e).val());
		newTotal += price * quan;
	});
	//nếu chưa nhập code thì thông báo
	if (_code == "") {
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
			title: 'Vui lòng nhập mã giảm giá'
		})
    }
	if (_code.length) {
		$.getJSON("/cart/UseDiscountCode", { code: _code, total_price: _totalprice},//truyền code giảm giá và tổng số tiền ban đầu(để tính số tiền được giảm tối đa: giảm giá theo %)xuống backend
			function (data, textStatus, jqXHR) {
				if (data.success) {
					$("#discount").attr("data-price", data.discountPrice);//thêm data-price vào thẻ div "Giảm giá:"
                    updateOrderPrice();
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
                        title: 'Đã áp dụng mã giảm giá'
					})
                    // Hiển thị thông báo
                } else {
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
                        title: 'Đã có lỗi, không thể sử dụng mã giảm giá'
                    })
                }
            }
		);
	}
})
// Button giảm số lượng
$(".btn-dec").click(function (ev) {
	$(".btn-inc").removeClass('no-pointer-events');
	var quanInput = $(ev.currentTarget).next();
	var id = quanInput.data("id");
	var quan = Number(quanInput.val());
	if (quan > 1) {
		quan = quan - 1;
		Cookie.set("product_" + id, quan, 10);//set sản phẩm lên cookie, sản phẩm tự xóa sau 10 ngày
		quanInput.val(quan);//update số lượng lên view
		updateOrderPrice();//update giá lên view
	}
});
//Nhập số lượng vào ô nhập và thay đổi
function Update_quan_mouse_ev() {
	var id = $('.qty-input').data("id");
	//no-pointer-events không cho click thực hiện 1 action
	$(".btn-inc").removeClass('no-pointer-events');
	var quan = $('.qty-input').val();
	if (quan != "")
	{
		Cookie.set("product_" + id, quan, 10);//set sản phẩm lên cookie, sản phẩm tự xóa sau 10 ngày
		updateOrderPrice();
	}
}
//các animation khi thay đổi số lượng sản phẩm ở ô input
$(".qty-input").mouseleave(function () {
	Update_quan_mouse_ev()
});
$(".qty-input").mouseover(function () {
	Update_quan_mouse_ev()
});
$(".qty-input").change(function (ev) {
	Update_quan_mouse_ev()
})
$(".qty-input").mouseout(function (ev) {
	Update_quan_mouse_ev()
})
// Button tăng số lượng
$(".btn-inc").click(function (ev) {
	var maxquan = $('.qty-input').attr('max');
	var quanInput = $(ev.currentTarget).prev();
	var id = quanInput.data("id");
	var quan = Number(quanInput.val());
	if (quan < 1)//số lượng sản phẩm không được = 0 
	{
		quan = 1;
		Cookie.set("product_" + id, quan, 10);
		quanInput.val(quan);
		updateOrderPrice();
	}
	else if (quan >= maxquan)
	{
		//nếu quan >= maxquan(get từ trên front) thì quan lấy luôn số lượng của maxquan và thông báo cho user về số lượng đã đạt giới hạn
		$(".btn-inc").addClass('no-pointer-events');
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
			title: 'Số lượng đã đạt giới hạn'
		})
    }
	else
	{
		quan = quan + 1;
		Cookie.set("product_" + id, quan, 10);
		quanInput.val(quan);
		updateOrderPrice();
    }
});