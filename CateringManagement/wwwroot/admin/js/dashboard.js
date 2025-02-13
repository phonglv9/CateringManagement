﻿$(document).ready(function () {
    GetFillterReprort();
    $("#btnExport").click(function () {
        let table = document.getElementsByTagName("table");
        TableToExcel.convert(table[0], {
            name: `Report.xlsx`,
            sheet: {
                name: 'Report'
            }
        });
    });
    $('#date_start').change(function () {
        var startDate = new Date(this.value);
        var oneMonthAfterStart = new Date(startDate);
        oneMonthAfterStart.setMonth(oneMonthAfterStart.getMonth() + 1);
        var endDateInput = document.getElementById("date_end");
        endDateInput.value = "";
        endDateInput.setAttribute("min", this.value);
        endDateInput.setAttribute("max", oneMonthAfterStart.toISOString().split('T')[0]);
    })
});
var myChart;
function GetFillterReprort() {

    var dateStart = moment($('#date_start').val());
    var dateEnd = moment($('#date_end').val());

    if (!dateStart.isValid() || !dateEnd.isValid() ) {
        toastr.error('No value date !', "Error");
        return;
    }
    if (dateStart.isAfter(dateEnd)) {
        toastr.error('The start date must be less than the end date', "Error");
        return;
    }
    $('#text-date').text(`From ${dateStart.format('DD/MM/YYYY')} to ${dateEnd.format('DD/MM/YYYY')}:`);
 
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
                var totalMoney = item.totalMoney;
                var order = item.order;

                var row = $('<tr>');
                row.append($('<td>').text(dateValue));
                row.append($('<td>').text(totalMoney));
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
                labels.push(moment(item.dateValue).format('DD/MM/YYYY '));
                orders.push(item.order);
                revenues.push(item.totalMoney);

                totalMoney += item.totalMoney;
                totalOrder += item.order;
            });
            $('#total_money').text(totalMoney);
            $('#total_order').text(totalOrder);
            var ctx = document.getElementById('myChart').getContext('2d');
            if (myChart) {
                myChart.destroy();
            }
            myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Oder',
                            data: orders,
                            backgroundColor: '#E31837',
                            borderColor: '#E31837',
                            borderWidth: 1

                        },
                        {
                            label: 'Revenue',
                              data: revenues,
                            backgroundColor: '#0039a6',
                            borderColor: '#0039a6',
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
                                    return value;
                                }
                            }
                        }]
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                var label = data.labels[tooltipItem.index];
                                label = moment(label).format('DD/MM/YYYY');
                                return label + ': ' + tooltipItem.yLabel;
                            }
                        }
                    }
                }
            });

        }
    });
}


