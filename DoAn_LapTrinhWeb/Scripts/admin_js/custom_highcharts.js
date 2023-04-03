$('#btntype_1').click(function () {
    $(this).addClass('color-active-chart');
    $('#btntype_2').removeClass('color-active-chart');
})
$('#btntype_2').click(function () {
    $(this).addClass('color-active-chart');
    $('#btntype_1').removeClass('color-active-chart');
})
$('#btntype_countorder_1').click(function () {
    $(this).addClass('color-active-chart');
    $('#btntype_countorder_2').removeClass('color-active-chart');
})
$('#btntype_countorder_2').click(function () {
    $(this).addClass('color-active-chart');
    $('#btntype_countorder_1').removeClass('color-active-chart');
})
$(document).ready(function () {
    let _id = 1
    $.ajax({
        type: "post",
        url: '/Dashboard/Revenue',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        success: function (data) {
            var Days = []
            var Total = []
            const Month = new Date();
            const lastDayOfMonth = new Date(Month.getFullYear(), Month.getMonth() + 1, 0);
            for (var i = 0; i < data.length; i++) {
                if (data[i].days < 10) {
                    Days.push('0' + data[i].days + '/' + (Month.getMonth() + 1));
                }
                else {
                Days.push(data[i].days + '/' + (Month.getMonth() + 1));
                }
                Total.push(data[i].total);
            }
            Highcharts.chart('sales_charts', {
                data: {
                    table: 'datatable'
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Doanh thu'
                },
                subtitle: {
                    text: '01/' + (Month.getMonth() + 1) + ' - ' + Month.getDate() + '/' + (Month.getMonth() + 1)
                },
                xAxis: {
                    categories: Days,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Đơn vị tính: Triệu đồng'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} VNĐ </b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                series: [{
                    name: 'Doanh thu',
                    color: "#3a77ca",
                    data: Total
                }],
            });
        }
    });
    //số đơn hàng
    $.ajax({
        type: "post",
        url: '/Dashboard/CountTotalOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        success: function (data) {
            var Time = []
            var TotalOrder = []
            const Month = new Date();
            const lastDayOfMonth = new Date(Month.getFullYear(), Month.getMonth() + 1, 0);
            for (var i = 0; i < data.length; i++) {
                if (data[i].days < 10) {
                    Time.push('0' + data[i].time + '/' + (Month.getMonth() + 1));
                }
                else {
                    Time.push(data[i].time + '/' + (Month.getMonth() + 1));
                }
                TotalOrder.push(data[i].counttotal);
            }
            Highcharts.chart('total_order_charts', {
                data: {
                    table: 'datatable'
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Số đơn hàng'
                },
                subtitle: {
                    text: '01/' + (Month.getMonth() + 1) + ' - ' + Month.getDate() + '/' + (Month.getMonth() + 1)
                },
                xAxis: {
                    categories: Time,
                    crosshair: true
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: 'Đơn vị tính: Đơn hàng'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} đơn hàng </b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                series: [{
                    name: 'Số đơn hàng',
                    color: "#3a77ca",
                    data: TotalOrder
                }],
            });
        }
    });
    //số lương sản phẩm bán ra
    $.getJSON("/Dashboard/CountQuantityProductlOrder", function (data) {
        let data1, data2, data3, data4, data5
        const Month = new Date();
        if (data.length == 0) {
            data1 = { name: 'không có sản phẩm nào', y: 0 }
        }
        else {
            for (var i = 0; i < data.length; i++) {
                if (data.length == 1) {
                    data1 = { name: data[0].name, y: data[0].count }
                }
                else if (data.length == 2) {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                }
                else if (data.length == 3) {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                    data3 = { name: data[2].name, y: data[2].count }
                }
                else if (data.length == 4) {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                    data3 = { name: data[2].name, y: data[2].count }
                    data4 = { name: data[3].name, y: data[3].count }
                }
                else {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                    data3 = { name: data[2].name, y: data[2].count }
                    data4 = { name: data[3].name, y: data[3].count }
                    data5 = { name: data[4].name, y: data[4].count }
                }
            }
        }
        Highcharts.chart('quantity_productorder_charts', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'TOP 5 sản phẩm bán chạy'
            },
            subtitle: {
                text: 'Ngày ' + Month.getDate() + '/' + (Month.getMonth() + 1)
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y} cái</b'
            },
            accessibility: {
                point: {
                    valueSuffix: '%'
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}: SL ({point.y})',
                        connectorColor: 'silver',
                        style: {
                            fontWeight: 'normal',
                            fontFamily: 'Roboto-Medium'
                        }
                    }
                }
            },
            series: [{
                name: '',
                data: [data1, data2, data3, data4, data5]
            }]
        });
    });
    //Khu vực đặt hàng nhiều nhất
    $.getJSON("/Dashboard/CountAddressOrder", function (data) {
        let data1, data2, data3, data4, data5
        const Month = new Date();
        if (data.length == 0) {
            data1 = { name: 'Không có', y: 0 }
        }
        else {
            for (var i = 0; i < data.length; i++) {
                if (data.length == 1) {
                    data1 = { name: data[0].name, y: data[0].count }
                }
                else if (data.length == 2) {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                }
                else if (data.length == 3) {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                    data3 = { name: data[2].name, y: data[2].count }
                }
                else if (data.length == 4) {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                    data3 = { name: data[2].name, y: data[2].count }
                    data4 = { name: data[3].name, y: data[3].count }
                }
                else {
                    data1 = { name: data[0].name, y: data[0].count }
                    data2 = { name: data[1].name, y: data[1].count }
                    data3 = { name: data[2].name, y: data[2].count }
                    data4 = { name: data[3].name, y: data[3].count }
                    data5 = { name: data[4].name, y: data[4].count }
                }
            }
        }
        Highcharts.chart('addresstorder_charts', {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'TOP 5 khu vực đặt hàng'
            },
            subtitle: {
                text: 'Ngày ' + Month.getDate() + '/' + (Month.getMonth() + 1)
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y} đơn hàng</b'
            },
            accessibility: {
                point: {
                    valueSuffix: '%'
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}: ({point.y}) ĐH',
                        connectorColor: 'silver',
                        style: {
                            fontWeight: 'normal',
                            fontFamily: 'Roboto-Medium'
                        }
                    }
                }
            },
            series: [{
                name: '',
                data: [data1, data2, data3, data4, data5]
            }]
        });
    });
});
//số lượng sản phẩm bán ra filer
var pie_count_product = function (_id) {
    $.ajax({
        type: "post",
        url: '/Dashboard/CountQuantityProductlOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        success: function (data) {
            let data1, data2, data3, data4, data5
            const Month = new Date();
            if (data.length == 0) {
                data1 = { name: 'không có sản phẩm nào', y: 0 }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data.length == 1) {
                        data1 = { name: data[0].name, y: data[0].count }
                    }
                    else if (data.length == 2) {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                    }
                    else if (data.length == 3) {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                        data3 = { name: data[2].name, y: data[2].count }
                    }
                    else if (data.length == 4) {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                        data3 = { name: data[2].name, y: data[2].count }
                        data4 = { name: data[3].name, y: data[3].count }
                    }
                    else {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                        data3 = { name: data[2].name, y: data[2].count }
                        data4 = { name: data[3].name, y: data[3].count }
                        data5 = { name: data[4].name, y: data[4].count }
                    }
                }
            }
            if (_id == 1) {
                $('#btntime_pie_product_1').addClass('color-active-chart');
                $('#btntime_pie_product_2,#btntime_pie_product_3,#btntime_pie_product_4').removeClass('color-active-chart');
                Highcharts.chart('quantity_productorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 sản phẩm bán chạy'
                    },
                    subtitle: {
                        text: 'Ngày ' + Month.getDate() + '/' + (Month.getMonth() + 1)
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y} cái</b'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: SL ({point.y})',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily: 'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
            else if (_id == 2) {
                $('#btntime_pie_product_2').addClass('color-active-chart');
                $('#btntime_pie_product_1,#btntime_pie_product_3,#btntime_pie_product_4').removeClass('color-active-chart');
                Highcharts.chart('quantity_productorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 sản phẩm bán chạy'
                    },
                    subtitle: {
                        text: (Month.getDate() - 7) + '/' + (Month.getMonth() + 1) + ' - ' + (Month.getDate()-1) + '/' + (Month.getMonth() + 1)
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>SL {point.y}</b'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: SL ({point.y})',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily: 'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
            else if (_id == 3) {
                $('#btntime_pie_product_3').addClass('color-active-chart');
                $('#btntime_pie_product_1,#btntime_pie_product_2,#btntime_pie_product_4').removeClass('color-active-chart');
                Highcharts.chart('quantity_productorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 sản phẩm bán chạy'
                    },
                    subtitle: {
                        text: 'Tháng '+ (Month.getMonth() + 1)
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y} cái</b'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: SL ({point.y})',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily: 'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
            else {
                $('#btntime_pie_product_4').addClass('color-active-chart');
                $('#btntime_pie_product_1,#btntime_pie_product_2,#btntime_pie_product_3').removeClass('color-active-chart');
                Highcharts.chart('quantity_productorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 sản phẩm bán chạy'
                    },
                    subtitle: {
                        text: 'Năm ' + Month.getFullYear()
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y} cái</b>'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: SL ({point.y})',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily:'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
        }
    });
};
//top 5 khu vực đặt hàng filter
var pie_count_address = function (_id) {
    $.ajax({
        type: "post",
        url: '/Dashboard/CountAddressOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        success: function (data) {
            let data1, data2, data3, data4, data5
            const Month = new Date();
            if (data.length == 0) {
                data1 = { name: 'không có', y: 0 }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    if (data.length == 1) {
                        data1 = { name: data[0].name, y: data[0].count }
                    }
                    else if (data.length == 2) {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                    }
                    else if (data.length == 3) {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                        data3 = { name: data[2].name, y: data[2].count }
                    }
                    else if (data.length == 4) {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                        data3 = { name: data[2].name, y: data[2].count }
                        data4 = { name: data[3].name, y: data[3].count }
                    }
                    else {
                        data1 = { name: data[0].name, y: data[0].count }
                        data2 = { name: data[1].name, y: data[1].count }
                        data3 = { name: data[2].name, y: data[2].count }
                        data4 = { name: data[3].name, y: data[3].count }
                        data5 = { name: data[4].name, y: data[4].count }
                    }
                }
            }
            if (_id == 1) {
                $('#btntime_pie_address_1').addClass('color-active-chart');
                $('#btntime_pie_address_2,#btntime_pie_address_3,#btntime_pie_address_4').removeClass('color-active-chart');
                Highcharts.chart('addresstorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 khu vực đặt hàng'
                    },
                    subtitle: {
                        text: 'Ngày ' + Month.getDate() + '/' + (Month.getMonth() + 1)
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y} đơn hàng</b'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: ({point.y}) ĐH',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily: 'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
            else if (_id == 2) {
                $('#btntime_pie_address_2').addClass('color-active-chart');
                $('#btntime_pie_address_1,#btntime_pie_address_3,#btntime_pie_address_4').removeClass('color-active-chart');
                Highcharts.chart('addresstorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 khu vực đặt hàng'
                    },
                    subtitle: {
                        text: (Month.getDate() - 7) + '/' + (Month.getMonth() + 1) + ' - ' + (Month.getDate()-1) + '/' + (Month.getMonth() + 1)
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y} đơn hàng</b'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: ({point.y}) ĐH',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily: 'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
            else if (_id == 3) {
                $('#btntime_pie_address_3').addClass('color-active-chart');
                $('#btntime_pie_address_1,#btntime_pie_address_2,#btntime_pie_address_4').removeClass('color-active-chart');
                Highcharts.chart('addresstorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 khu vực đặt hàng'
                    },
                    subtitle: {
                        text: 'Tháng ' + (Month.getMonth() + 1)
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y} đơn hàng</b'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: ({point.y}) ĐH',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily: 'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
            else {
                $('#btntime_pie_address_4').addClass('color-active-chart');
                $('#btntime_pie_address_1,#btntime_pie_address_2,#btntime_pie_address_3').removeClass('color-active-chart');
                Highcharts.chart('addresstorder_charts', {
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false,
                        type: 'pie'
                    },
                    title: {
                        text: 'TOP 5 khu vực đặt hàng'
                    },
                    subtitle: {
                        text: 'Năm ' + Month.getFullYear()
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>SL {point.y}</b>'
                    },
                    accessibility: {
                        point: {
                            valueSuffix: '%'
                        }
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}: ({point.y}) ĐH',
                                connectorColor: 'silver',
                                style: {
                                    fontWeight: 'normal',
                                    fontFamily: 'Roboto-Medium'
                                }
                            }
                        }
                    },
                    series: [{
                        name: '',
                        data: [data1, data2, data3, data4, data5]
                    }]
                });
            }
        }
    });
};
//Doanh thu: dạng cột
var columnchart = function (_id) {
    $.ajax({
        type: "post",
        url: '/Dashboard/Revenue',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id}),
        dataType: "json",
        success: function (data) {
            $('.type_sort_linechart').attr('onclick', 'linechart(' + _id + ')')
            $('.type_sort_columnchart').attr('onclick', 'columnchart(' + _id + ')')
            var Days = []
            var Total = []
            const Month = new Date();
            const lastDayOfMonth = new Date(Month.getFullYear(), Month.getMonth() + 1, 0);
            if (_id == 1) {
                $('#btntime_1,#btntype_1').addClass('color-active-chart');
                $('#btntime_2,#btntime_3,#btntype_2').removeClass('color-active-chart');
                for (var i = 0; i < data.length; i++) {
                    if (data[i].days < 10) {
                        Days.push('0' + data[i].days + '/' + (Month.getMonth() + 1));
                    }
                    else {
                        Days.push(data[i].days + '/' + (Month.getMonth() + 1));
                    }
                    Total.push(data[i].total);
                }
                Highcharts.chart('sales_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Doanh thu'
                    },
                    subtitle: {
                        text: '01/' + (Month.getMonth() + 1) + ' - ' + Month.getDate() + '/' + (Month.getMonth() + 1)
                    },
                    xAxis: {
                        categories: Days,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Triệu đồng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} VNĐ </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Doanh thu',
                        color: "#3a77ca",
                        data: Total
                    }],
                });
            } else if (_id == 2) {
                $('#btntime_2,#btntype_1').addClass('color-active-chart');
                $('#btntime_1,#btntime_3,#btntype_2').removeClass('color-active-chart');
                for (var i = 0; i < data.length; i++) {
                    Days.push('Tháng '+data[i].days);
                    Total.push(data[i].total);
                }
                Highcharts.chart('sales_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Doanh thu'
                    },
                    subtitle: {
                        text: '01/' + Month.getFullYear() + ' - ' + '12/' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Days,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Triệu đồng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} VNĐ </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Doanh thu',
                        color: "#3a77ca",
                        data: Total
                    }],
                });
            }
            else {
                $('#btntime_3,#btntype_1').addClass('color-active-chart');
                $('#btntime_1,#btntime_2,#btntype_2').removeClass('color-active-chart');
                for (var i = 0; i < data.length; i++) {
                    Days.push(data[i].days);
                    Total.push(data[i].total);
                }
                Highcharts.chart('sales_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Doanh thu'
                    },
                    subtitle: {
                        text: (Month.getFullYear() - 4) + ' - ' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Days,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Triệu đồng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} VNĐ </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Doanh thu',
                        color: "#3a77ca",
                        data: Total
                    }],
                });
            }
        }
    });
};
//Số đơn hàng: dạng cột
var columnchart_count_order = function (_id) {
    $.ajax({
        type: "post",
        url: '/Dashboard/CountTotalOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        success: function (data) {
            $('.type_sort_linechart_count_order').attr('onclick', 'linechart_count_order(' + _id + ')')
            $('.type_sort_columnchart_count_order').attr('onclick', 'columnchart_count_order(' + _id + ')')
            var Time = []
            var Totalorder = []
            const Month = new Date();
            const lastDayOfMonth = new Date(Month.getFullYear(), Month.getMonth() + 1, 0);
            if (_id == 1) {
                $('#btntime_countorder_1,#btntype_countorder_1').addClass('color-active-chart');
                $('#btntime_countorder_2,#btntime_countorder_3,#btntype_countorder_2').removeClass('color-active-chart');
                for (var i = 0; i < data.length; i++) {
                    if (data[i].time < 10) {
                        Time.push('0' + data[i].time + '/' + (Month.getMonth() + 1));
                    }
                    else {
                        Time.push(data[i].time + '/' + (Month.getMonth() + 1));
                    }
                    Totalorder.push(data[i].counttotal);
                }
                Highcharts.chart('total_order_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Số đơn hàng'
                    },
                    subtitle: {
                        text: '01/' + (Month.getMonth() + 1) + ' - ' + lastDayOfMonth.getDate() + '/' + (Month.getMonth() + 1)
                    },
                    xAxis: {
                        categories: Time,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: đơn hàng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} đơn hàng </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Số đơn hàng',
                        color: "#3a77ca",
                        data: Totalorder
                    }],
                });
            } else if (_id == 2) {
                $('#btntime_countorder_2,#btntype_countorder_1').addClass('color-active-chart');
                $('#btntime_countorder_1,#btntime_countorder_3,#btntype_countorder_2').removeClass('color-active-chart');
                for (var i = 0; i < data.length; i++) {
                    Time.push('Tháng ' + data[i].time);
                    Totalorder.push(data[i].counttotal);
                }
                Highcharts.chart('total_order_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Số đơn hàng'
                    },
                    subtitle: {
                        text: '01/' + Month.getFullYear() + ' - ' + '12/' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Time,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Đơn hàng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} đơn hàng </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Số đơn hàng',
                        color: "#3a77ca",
                        data: Totalorder
                    }],
                });
            }
            else {
                $('#btntime_countorder_3,#btntype_countorder_1').addClass('color-active-chart');
                $('#btntime_countorder_1,#btntime_countorder_2,#btntype_countorder_2').removeClass('color-active-chart');
                for (var i = 0; i < data.length; i++) {
                    Time.push(data[i].time);
                    Totalorder.push(data[i].counttotal);
                }
                Highcharts.chart('total_order_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: 'Số đơn hàng'
                    },
                    subtitle: {
                        text: (Month.getFullYear() - 4) + ' - ' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Time,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Đơn hàng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} đơn hàng </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Số đơn hàng',
                        color: "#3a77ca",
                        data: Totalorder
                    }],
                });
            }
        }
    });
};
//Doanh thu: dạng miền
var linechart = function (_id) {
    $.ajax({
        type: "post",
        url: '/Dashboard/Revenue',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        success: function (data) {
            $('.type_sort_columnchart').attr('onclick', 'columnchart(' + _id + ')')
            var Days = []
            var Total = []
            var Numorder = []
            const Month = new Date();
            const lastDayOfMonth = new Date(Month.getFullYear(), Month.getMonth() + 1, 0);
            if (_id == 1) {

                for (var i = 0; i < data.length; i++) {
                    if (data[i].days < 10) {
                        Days.push('0' + data[i].days + '/' + (Month.getMonth() + 1));
                    }
                    else {
                        Days.push(data[i].days + '/' + (Month.getMonth() + 1));
                    }
                    Total.push(data[i].total);
                }
                Highcharts.chart('sales_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'line'
                    },
                    title: {
                        text: 'Tổng doanh thu ngày'
                    },
                    subtitle: {
                        text: '01/' + (Month.getMonth() + 1) + ' - ' + Month.getDate() + '/' + (Month.getMonth() + 1)
                    },
                    xAxis: {
                        categories: Days,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Triệu đồng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} VNĐ </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Tổng doanh thu',
                        color: "#3a77ca",
                        data: Total
                    }],
                });
            } else if (_id == 2) {
                for (var i = 0; i < data.length; i++) {
                    Days.push('Tháng ' + data[i].days);
                    Total.push(data[i].total);
                }
                Highcharts.chart('sales_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'line'
                    },
                    title: {
                        text: 'Tổng doanh thu tháng'
                    },
                    subtitle: {
                        text: '01/' + Month.getFullYear() + ' - ' + '12/' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Days,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Triệu đồng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} VNĐ </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Tổng doanh thu',
                        color: "#3a77ca",
                        data: Total
                    }],
                });
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    Days.push(data[i].days);
                    Total.push(data[i].total);
                }
                Highcharts.chart('sales_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'line'
                    },
                    title: {
                        text: 'Tổng doanh thu năm'
                    },
                    subtitle: {
                        text: (Month.getFullYear() - 4) + ' - ' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Days,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Triệu đồng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} VNĐ </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Tổng doanh thu',
                        color: "#3a77ca",
                        data: Total
                    }],
                });
            }
        }
    });
};
//Số đơn hàng: dạng miền
var linechart_count_order = function (_id) {
    $.ajax({
        type: "post",
        url: '/Dashboard/CountTotalOrder',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        success: function (data) {
            $('.type_sort_columnchart_count_order').attr('onclick', 'columnchart_count_order(' + _id + ')')
            var Time = []
            var Totalorder = []
            const Month = new Date();
            const lastDayOfMonth = new Date(Month.getFullYear(), Month.getMonth() + 1, 0);
            if (_id == 1) {
                $('#btntime_countorder_1').addClass('color-active-chart');
                $('#btntime_countorder_2,#btntime_countorder_3').removeClass('color-active-chart');
                for (var i = 0; i < data.length; i++) {
                    if (data[i].time < 10) {
                        Time.push('0' + data[i].time + '/' + (Month.getMonth() + 1));
                    }
                    else {
                        Time.push(data[i].time + '/' + (Month.getMonth() + 1));
                    }
                    Totalorder.push(data[i].counttotal);
                }
                Highcharts.chart('total_order_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'line'
                    },
                    title: {
                        text: 'Số đơn hàng'
                    },
                    subtitle: {
                        text: '01/' + (Month.getMonth() + 1) + ' - ' + Month.getDate() + '/' + (Month.getMonth() + 1)
                    },
                    xAxis: {
                        categories: Time,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: đơn hàng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} đơn hàng </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Số đơn hàng',
                        color: "#3a77ca",
                        data: Totalorder
                    }],
                });
            } else if (_id == 2) {
                for (var i = 0; i < data.length; i++) {
                    Time.push('Tháng ' + data[i].time);
                    Totalorder.push(data[i].counttotal);
                }
                Highcharts.chart('total_order_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'line'
                    },
                    title: {
                        text: 'Số đơn hàng'
                    },
                    subtitle: {
                        text: '01/' + Month.getFullYear() + ' - ' + '12/' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Time,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Đơn hàng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} đơn hàng </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Số đơn hàng',
                        color: "#3a77ca",
                        data: Totalorder
                    }],
                });
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    Time.push(data[i].time);
                    Totalorder.push(data[i].counttotal);
                }
                Highcharts.chart('total_order_charts', {
                    data: {
                        table: 'datatable'
                    },
                    chart: {
                        type: 'line'
                    },
                    title: {
                        text: 'Số đơn hàng'
                    },
                    subtitle: {
                        text: (Month.getFullYear() - 4) + ' - ' + Month.getFullYear()
                    },
                    xAxis: {
                        categories: Time,
                        crosshair: true
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Đơn vị tính: Đơn hàng'
                        }
                    },
                    tooltip: {
                        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' + '<td style="padding-left:2px;"><b>{point.y} đơn hàng </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    series: [{
                        name: 'Số đơn hàng',
                        color: "#3a77ca",
                        data: Totalorder
                    }],
                });
            }
        }
    });
};