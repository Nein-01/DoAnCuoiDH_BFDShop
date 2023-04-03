//sổ địa chỉ xuống để chọn
$(document).ready(function () {
    $('#province').change(function () {
        $('#district').removeClass('cursor-disable');
        $('#district').removeAttr('disabled');
        $.get("/Account/GetDistrictsList", { province_id: $('#province').val() }, function (data) {
            $('#district').html("<option value>Quận/Huyện</option>");
            $.each(data, function (index, row) {
                $('#district').append("'<option value='" + row.district_id + "'>" + row.type + " " + row.district_name + "</option>")
            });
        });
    })
    $('#district').change(function () {
        $('#ward').removeClass('cursor-disable');
        $('#ward').removeAttr('disabled');
        $.get("/Account/GetWardsList", { district_id: $('#district').val() }, function (data) {
            $('#ward').html("<option value>Phường/ Xã</option>");
            $.each(data, function (index, row) {
                $('#ward').append("<option value='" + row.ward_id + "'>" + row.type + " " + row.ward_name + "</option>")
            });
        });
    })
});