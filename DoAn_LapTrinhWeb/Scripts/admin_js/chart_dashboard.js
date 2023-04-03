$(document).ready(function () {
        var day_data =
            [
            { "period": "2012-10-01", "licensed": 3407, "sorned": 660 },
            { "period": "2012-09-30", "licensed": 3351, "sorned": 629 },
            { "period": "2012-09-29", "licensed": 3269, "sorned": 618 },
            { "period": "2012-09-20", "licensed": 3246, "sorned": 661 },
            { "period": "2012-09-19", "licensed": 3257, "sorned": 667 },
            { "period": "2012-09-18", "licensed": 3248, "sorned": 627 },
            { "period": "2012-09-17", "licensed": 3171, "sorned": 660 },
            { "period": "2012-09-16", "licensed": 3171, "sorned": 676 },
            { "period": "2012-09-15", "licensed": 3201, "sorned": 656 },
            { "period": "2012-09-10", "licensed": 3215, "sorned": 622 }
        ];
        Morris.Bar({
            element: 'graph',
            data: day_data,
            hideHover:'auto',
            xkey: 'period',
            ykeys: ['licensed', 'sorned'],
            labels: ['Licensed', 'SORN'],
            xLabelAngle: 60
        });

    var chart = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,
        title: {
            text: "Top 5 sản phẩm bán chạy",
            horizontalAlign: "center",
            fontFamily:"Roboto-Medium"
        },
        data: [{
            type: "doughnut",
            startAngle: 60,
            innerRadius: 70,
            indexLabelFontSize: 15,
            indexLabel: "{label} - #percent%",
            toolTipContent: "<b>{label}:</b> {y} (#percent%)",
            dataPoints: [
                { y: 67, label: "Inbox" },
                { y: 28, label: "Archives" },
                { y: 10, label: "Labels" },
                { y: 7, label: "Drafts" },
                { y: 15, label: "Trash" },
                { y: 6, label: "Spam" }
            ]
        }]
    });
    chart.render();
    var chart2 = new CanvasJS.Chart("chartContainer2", {
        animationEnabled: true,
        title: {
            text: "Top 5 khu vực đặt hàng",
            horizontalAlign: "center",
            fontFamily: "Roboto-Medium"
        },
        data: [{
            type: "doughnut",
            startAngle: 60,
            innerRadius: 70,
            indexLabelFontSize: 15,
            indexLabel: "{label} - #percent%",
            toolTipContent: "<b>{label}:</b> {y} (#percent%)",
            dataPoints: [
                { y: 67, label: "Inbox" },
                { y: 28, label: "Archives" },
                { y: 10, label: "Labels" },
                { y: 7, label: "Drafts" },
                { y: 15, label: "Trash" },
                { y: 6, label: "Spam" }
            ]
        }]
    });
    chart2.render();
})
