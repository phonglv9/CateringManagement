$(document).ready(function () {
    loadDataOrders();

    $('#add-btn').on('click', function () {
        showAddModal();
    });

    $('#save-btn').on('click', function () {
        addOrder();
    });

    $('#update-btn').on('click', function () {
        updateOrder();
    });
});

function loadDataOrders() {
    $('#orders-table').DataTable({
        "ajax": {
            "url": "/Order/GetListOrders",
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "Code" },
            { "data": "CreatedTime" },
            { "data": "PickupDate" },
            { "data": "CustomerName" },
            { "data": "CustomerPhone" },
            { "data": "TotalPrice" },
            { "data": "SellPrice" },
            { "data": "Status" },
            {
                "data": null, "render": function (data, type, row) {
                    let html = ``;
                    html += `<button type="button" class="btn btn-primary m-1" title="View detail" onclick="showDetailModal('${row.Id}')"><i class="fas fa-eye"></i></i></button>`;
                    html += `<button type="button" class="btn btn-warning m-1" title="Edit" onclick="showEditModal('${row.Id}')"><i class="fas fa-edit"></i></i></button>`;
                    if (row.Status != 'Done') {
                        html += `<button type="button" class="btn btn-success m-1" title="Complete" onclick="showCompleteModal('${row.Id}')"><i class="fas fa-check"></i></i></button>`;
                    }
                    html += `<button type="button" class="btn btn-danger m-1" title="Delete" onclick="showDeleteModal('${row.Id}')"><i class="fas fa-trash"></i></i></button>`;
                    return html;
                }
            },

        ]
    });
}

function loadDataTable() {
    $('#orders-table').DataTable().ajax.reload();
}

function showAddModal() {
    $('#customer-name').val('');
    $('#customer-phone').val('');
    $('#pickup-date').val('');
    $('#order-price').val(0);
    $('#sell-price').val(0);
    $("#select-meal").val("0").change();
    $('#meal-price').val('');
    $('#quantity').val('');
    $('#table-row-count').val(0);
    $('#orders-table-body').html(`<tr id="orders-table-no-row"><td colspan="5"align="center">No meals</td></tr>`);

    $.ajax({
        url: "/Order/GetSimpleListMeals",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                var htmlSelect = '<option value="0" selected>Select Meal</option>';
                $.each(result.data, function (key, item) {
                    htmlSelect += `<option value="${item.id}" id="meal-option-${item.id}" data-price="${item.price}">${item.name}</option>`;
                });
                $('#select-meal').html(htmlSelect);

                $('#add-order-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function changeMeal(mode = '') {
    let selectedMeal = $('#select-meal' + mode).find(":selected");
    if (selectedMeal.val() == 0) {
        $('#meal-price' + mode).val('');
    }
    else {
        $('#meal-price' + mode).val(selectedMeal.attr('data-price'));
    }
}


function addMealToTable(mode = '') {
    let selectedMeal = $('#select-meal' + mode).find(":selected");
    let mealId = selectedMeal.val();
    let mealName = selectedMeal.text();
    let mealPrice = parseFloat(selectedMeal.attr("data-price"));

    if (mealId == 0 || $('#quantity' + mode).val() == '' || $('#quantity' + mode).val() <= 0) {
        return;
    }

    let quantity = parseInt($('#quantity' + mode).val());
    let totalMealPrice = parseFloat(mealPrice * quantity).toFixed(2);

    // check no row
    let orderPrice = parseFloat($('#order-price' + mode).val());
    let rowCount = parseInt($('#table-row-count' + mode).val());
    if (rowCount == 0) {
        $('#orders-table-no-row' + mode).remove();
    }
    $('#table-row-count' + mode).val(rowCount + 1);

    // increase price
    orderPrice = (parseFloat(orderPrice) + parseFloat(totalMealPrice)).toFixed(2);
    $('#order-price' + mode).val(orderPrice);
    $('#sell-price' + mode).val((orderPrice * 100 / 35).toFixed(2));

    let htmlRow = `<tr id="meal-row${mode}-${mealId}" data-row-id="${mealId}" class="has-data" data-row-quantity="${quantity}">`;
    htmlRow += `<td>${mealName}</td>`;
    htmlRow += `<td>${mealPrice}</td>`;
    htmlRow += `<td>${quantity}</td>`;;
    htmlRow += `<td>${totalMealPrice}</td>`;
    htmlRow += `<td><button type="button" class="btn btn-danger" title="Delete" onclick="removeMealFromTable('${mealId}', ${totalMealPrice}, '${mode}')"><i class="fas fa-trash"></i></i></button></td>`;
    htmlRow += `</tr>`;

    // add to table
    $('#orders-table-body' + mode).append(htmlRow);

    // disable selected option of select
    $(`#meal-option${mode}-` + mealId).attr('disabled', 'disabled');

    // reset select
    $("#select-meal" + mode).val("0").change();
    $('#quantity' + mode).val('');
}

function removeMealFromTable(id, removedPrice, mode = '') {
    // remove row
    $(`#meal-row${mode}-` + id).remove();

    // minus counter
    let rowCount = parseInt($('#table-row-count' + mode).val());
    console.log(rowCount);
    $('#table-row-count' + mode).val(--rowCount);

    // decrease price
    let orderPrice = parseFloat($('#order-price' + mode).val());
    orderPrice -= removedPrice;
    $('#order-price' + mode).val(orderPrice.toFixed(2));
    $('#sell-price' + mode).val((orderPrice * 100 / 35).toFixed(2));

    // check no row
    if (rowCount == 0) {
        $('#orders-table-body' + mode).html(`<tr id="orders-table-no-row${mode}"><td colspan="5"align="center">No meals</td></tr>`);
    }

    // enable selected option of select
    $(`#meal-option${mode}-` + id).removeAttr('disabled');
}

function addOrder() {
    var mealArr = [];

    if ($('#customer-name').val() == '') {
        toastr.error("Please enter customer name", "Error");
        return;
    }
    if ($('#customer-phone').val() == '') {
        toastr.error("Please enter customer phone", "Error");
        return;
    }
    if ($('#pickup-date').val() == '') {
        toastr.error("Please enter pickup date", "Error");
        return;
    }

    $('#orders-table-body tr.has-data').each(function (index, value) {
        mealArr.push({
            MealId: $(this).attr('data-row-id'),
            Quantity: parseInt($(this).attr('data-row-quantity'))
        });
    });

    if (mealArr.length == 0) {
        toastr.error("Please add the meal", "Error");
        return;
    }

    var dataObj = {
        CustomerName: $('#customer-name').val(),
        CustomerPhone: $('#customer-phone').val(),
        PickupDate: $('#pickup-date').val(),
        Meals: mealArr
    };

    $.ajax({
        url: "/Order/AddOrder",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#add-order-modal').modal('hide');
                loadDataTable();
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function showDetailModal(id) {
    $.ajax({
        url: "/Order/GetDetail/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                $('#customer-name-detail').html(result.data.customerName);
                $('#customer-phone-detail').html(result.data.customerPhone);
                $('#created-time-detail').html(result.data.createdTime);
                $('#pickup-date-detail').html(result.data.pickupDate);
                $('#status-detail').html(result.data.status);
                $('#order-price-detail').val(result.data.totalPrice);
                $('#sell-price-detail').val(result.data.sellPrice);

                let htmlRow = ``;
                let meals = result.data.meals;
                $.each(meals, function (index, item) {
                    htmlRow += `<tr>`;
                    htmlRow += `<td>${item.name}</td>`;
                    htmlRow += `<td>${item.price}</td>`;
                    htmlRow += `<td>${item.quantity}</td>`;
                    htmlRow += `<td>${item.totalPrice}</td>`;
                    htmlRow += `</tr>`;
                });

                $('#detail-orders-table-body').html(htmlRow);
                $('#detail-order-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function getDetailForEditModal(id) {
    $.ajax({
        url: "/Order/GetDetail/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                $('#customer-name-edit').val(result.data.customerName);
                $('#customer-phone-edit').val(result.data.customerPhone);
                var pickupDate = result.data.pickupDateDT.split('T')[0];
                $('#pickup-date-edit').val(pickupDate);
                $('#order-price-edit').val(result.data.totalPrice);
                $('#sell-price-edit').val(result.data.sellPrice);

                let htmlRow = ``;
                let meals = result.data.meals;
                $.each(meals, function (index, item) {
                    htmlRow += `<tr id="meal-row-edit-${item.mealId}" data-row-id="${item.mealId}" class="has-data" data-row-quantity="${item.quantity}">`;
                    htmlRow += `<td>${item.name}</td>`;
                    htmlRow += `<td>${item.price}</td>`;
                    htmlRow += `<td>${item.quantity}</td>`;
                    htmlRow += `<td>${item.totalPrice}</td>`;
                    htmlRow += `<td><button type="button" class="btn btn-danger" title="Delete" onclick="removeMealFromTable('${item.mealId}', ${item.totalPrice}, '-edit')"><i class="fas fa-trash"></i></i></button></td>`;
                    htmlRow += `</tr>`;

                    // disable selected option of select
                    $('#meal-option-edit-' + item.mealId).attr('disabled', 'disabled');
                });

                $('#table-row-count-edit').val(meals.length);
                $('#orders-table-body-edit').html(htmlRow);
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function showEditModal(id) {
    $("#select-meal-edit").val("0").change();
    $('#quantity-edit').val('');
    $('#id-edit').val(id);

    getDetailForEditModal(id);

    $.ajax({
        url: "/Order/GetSimpleListMeals",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                var htmlSelect = '<option value="0" selected>Select Meal</option>';
                $.each(result.data, function (key, item) {
                    htmlSelect += `<option value="${item.id}" id="meal-option-edit-${item.id}" data-price="${item.price}">${item.name}</option>`;
                });
                $('#select-meal-edit').html(htmlSelect);

                $('#edit-order-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function updateOrder(id) {
    var mealArr = [];

    if ($('#customer-name-edit').val() == '') {
        toastr.error("Please enter customer name", "Error");
        return;
    }
    if ($('#customer-phone-edit').val() == '') {
        toastr.error("Please enter customer phone", "Error");
        return;
    }
    if ($('#pickup-date-edit').val() == '') {
        toastr.error("Please enter pickup date", "Error");
        return;
    }

    $('#orders-table-body-edit tr.has-data').each(function (index, value) {
        mealArr.push({
            MealId: $(this).attr('data-row-id'),
            Quantity: parseInt($(this).attr('data-row-quantity'))
        });
    });

    if (mealArr.length == 0) {
        toastr.error("Please add the meal", "Error");
        return;
    }

    var dataObj = {
        CustomerName: $('#customer-name-edit').val(),
        CustomerPhone: $('#customer-phone-edit').val(),
        PickupDate: $('#pickup-date-edit').val(),
        Meals: mealArr
    };

    $.ajax({
        url: "/Order/UpdateOrder/" + $('#id-edit').val(),
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#edit-order-modal').modal('hide');
                loadDataTable();
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function showDeleteModal(id) {
    $('#id-delete').val(id);
    $('#delete-order-modal').modal('show');
}

function deleteOrder() {
    $.ajax({
        url: "/Order/DeleteOrder/" + $('#id-delete').val(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#delete-order-modal').modal('hide');
                loadDataTable();
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function showCompleteModal(id) {
    $('#id-complete').val(id);
    $('#complete-order-modal').modal('show');
}

function completeOrder() {
    $.ajax({
        url: "/Order/CompleteOrder/" + $('#id-complete').val(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#complete-order-modal').modal('hide');
                loadDataTable();
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}