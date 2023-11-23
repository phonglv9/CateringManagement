
var myChart;
function GetFillterReprort() {
    var dateStart = moment($('#date_start').val()).format('DD/MM/YYYY');
    var dateEnd = moment($('#date_end').val()).format('DD/MM/YYYY');
    $('#text-date').text(`Từ ${dateStart} đến ${dateEnd}:`);
    var dateStart = moment($('#date_start').val());
    var dateEnd = moment($('#date_end').val());

    if (dateStart.isAfter(dateEnd)) {
        MessageError('The start date must be less than the end date');
        return;
    }
    $('#text-date').text(`Từ ${dateStart.format('DD/MM/YYYY')} đến ${dateEnd.format('DD/MM/YYYY')}:`);
    $.ajax({
        url: '/Home/GetReportData',
        type: 'GET',
        data: { dateStart: $('#date_start').val(), dateEnd: $('#date_end').val() },
        dataType: 'json',
        success: function (data) {

            //Table
            var tbody = $('.datatable_report');
            tbody.empty();

            $.each(data, function (i, item) {
                var dateValue = moment(item.dateValue).format('DD/MM/YYYY');
                var totalMoney = item.totalMoney.toLocaleString('vi-VN');
                var order = item.order.toLocaleString('vi-VN');

                var row = $('<tr>');
                row.append($('<td>').text(dateValue));
                row.append($('<td>').text(totalMoney + ' đ'));
                row.append($('<td>').text(order));

                tbody.append(row);
            });
            ////
            var labels = [];
            var orders = [];
            var revenues = [];
            var totalMoney = 0;
            var totalOrder = 0;
            $.each(data, function (i, item) {
                labels.push(moment(item.dateValue).format('DD/MM/YYYY ')); // format lại label
                orders.push(item.order);
                revenues.push(item.totalMoney);

                totalMoney += item.totalMoney;
                totalOrder += item.order;
            });
            $('#total_money').text(totalMoney.toLocaleString('vi-VN') + ' đ');
            $('#total_order').text(totalOrder);
            var ctx = document.getElementById('myChart').getContext('2d');
            if (myChart) {
                myChart.destroy();
            }
            myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Oder',
                            data: orders,

                            backgroundColor: 'rgba(255, 99, 132, 0.5)',
                            borderColor: 'rgba(255, 99, 132, 1)',
                            borderWidth: 1

                        },
                        {
                            label: 'Revenue',
                              data: revenues,
                            backgroundColor: 'rgba(54, 162, 235, 0.5)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    return value.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                                }
                            }
                        }]
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                console.log(tooltipItem);
                                console.log(data);
                                var label = data.labels[tooltipItem.index];
                                label = moment(label).format('DD/MM/YYYY');
                                //if (reportType === 'revenue') {
                                //    return label + ': ' + tooltipItem.yLabel.toLocaleString('vi-VN', {style: 'currency', currency: 'VND' });
                                //} else {
                                //    return label + ': Đơn ' + tooltipItem.yLabel;
                                //}
                                return label + ': ' + tooltipItem.yLabel.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                            }
                        }
                    }
                }
            });

        }
    });
}
$(document).ready(function () {

    GetFillterReprort();

});

