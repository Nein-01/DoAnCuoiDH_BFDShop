/*
 * Javascript xử lý chung cho toàn bộ trang web
 */
(function () {
	// Khởi tạo thông tin giỏ hàng khi refresh trang
	var cartEle = $("#goToCart");
	if (cartEle.length) {
		var cartCount = Cookie.countWithPrefix("product");
		if (cartCount > 0) {
			$("#lblCartCount").text(cartCount);
		}
	}
})();