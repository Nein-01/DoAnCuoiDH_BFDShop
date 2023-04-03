
//http://www.mredkj.com/javascript/nfbasic.html
var Price = function (price, new_price) {
    _price = price
    _new_price = new_price
}
function addPeriod(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    }
    return x1 + x2;
}

$(document).ready(function () {
    $('#price-detail').text(addPeriod(_price))
    $('#new-price-detail').text(addPeriod(_price))
})